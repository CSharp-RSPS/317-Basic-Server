using RSPS.src.entity.player;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.Authentication
{
    public class LoginProtocolDecoder
    {


        public static void DecodeLogin(Connection connection, out Player? player)
        {
            player = null;

            //Console.WriteLine("Buffer Size: {0}", connection.buffer.Length);
            PacketReader readPacket;
            switch (connection.ConnectionState)
            {
                case ConnectionState.ConnectionRequest://connection state
                    readPacket = Packet.CreatePacketReader(connection.Buffer);

                    int opcode = readPacket.ReadByte() & 0xff;
                    int nameHash = readPacket.ReadByte() & 0xff; // used for login servers, not sure how it works

                    if (opcode == 14)
                    {
                        //Console.WriteLine("opcode = game");
                        //Console.WriteLine("NameHash: {0}", nameHash);
                        connection.ConnectionState = ConnectionState.Authenticate;

                        long dummyData = 0;

                        using MemoryStream stream = new(17);
                        stream.Write(BitConverter.GetBytes(dummyData));
                        stream.WriteByte(0); // Response code
                        stream.Write(BitConverter.GetBytes(new Random().NextInt64())); // Server session key
                        connection.Send(stream.GetBuffer());
                    }
                    if (opcode == 15)
                    {
                        Console.WriteLine("opcode = update");

                    }
                    break;

                case ConnectionState.Authenticate://login state
                    readPacket = Packet.CreatePacketReader(connection.Buffer);

                    //Console.WriteLine("We are inside the login state");
                    int loginType = readPacket.ReadByte();
                    //Console.WriteLine("LoginType: {0}", loginType);
                    if (loginType != 16 && loginType != 18)
                    {
                        Console.WriteLine("Invalid login Type: {0}", loginType);
                        PacketHandler.SendPacket(connection, new SendLoginResponse(LoginResponse.GameWasUpdated));
                        connection.Dispose();
                        break;
                    }
                    int blockLength = readPacket.ReadByte();//loginSize = 76
                    //Console.WriteLine("Block Length is {0}", blockLength);
                    if (connection.Buffer.Length < blockLength)
                    {
                        Console.WriteLine("State buffer length is less than block length");
                    }
                    int magicId = readPacket.ReadByte();//255

                    if (magicId != 255)
                    {
                        Console.WriteLine("Invalid magic ID");
                        PacketHandler.SendPacket(connection, new SendLoginResponse(LoginResponse.SessionRejected));
                        connection.Dispose();
                        break;
                    }
                    //Console.WriteLine("Magic ID: {0}", magicId);

                    int clientVersion = readPacket.ReadShort();
                    //Console.WriteLine("Client Version {0}", clientVersion);//needs to be 317. Bitconvert works
                    if (clientVersion != 317)
                    {
                        Console.WriteLine("Incorrect client version");
                        PacketHandler.SendPacket(connection, new SendLoginResponse(LoginResponse.GameWasUpdated));
                        connection.Dispose();
                        break;
                    }

                    readPacket.ReadByte();//High/low memory version

                    // Skip the CRC keys.
                    for (int i = 0; i < 9; i++)
                    {
                        readPacket.ReadInt();
                    }

                    int RSABlock = readPacket.ReadByte(); //Skip RSA block length
                    //Console.WriteLine("RSA Block {0}", RSABlock);

                    //Validate that the RSA block was decoded properly
                    int rsaOpcode = readPacket.ReadByte();
                    //Console.WriteLine("RSA Opcode: {0}", rsaOpcode);

                    if (rsaOpcode != 10)
                    {
                        Console.WriteLine("Unable to decode RSA block properly!");
                        PacketHandler.SendPacket(connection, new SendLoginResponse(LoginResponse.SessionRejected));
                        connection.Dispose();
                        break;
                    }

                    // Set up the ISAAC ciphers.
                    long clientHalf = readPacket.ReadLong(); // Client session key
                    long serverHalf = readPacket.ReadLong(); // Server session key

                    int[] isaacSeed = { (int)(clientHalf >> 32), (int)clientHalf, (int)(serverHalf >> 32), (int)serverHalf };
                    //Console.WriteLine(isaacSeed[0] + " , " + isaacSeed[1] + " , " + isaacSeed[2] + " , " + isaacSeed[3]);
                    connection.NetworkDecryptor = new ISAACCipher(isaacSeed);
                    //Console.WriteLine("Decryptor: " + connection.NetworkDecryptor);
                    for (int i = 0; i < 4; i++)
                    {
                        isaacSeed[i] += 50;
                    }
                    connection.NetworkEncryptor = new ISAACCipher(isaacSeed);
                    //Console.WriteLine("Encryptor: " + connection.NetworkEncryptor);
                    //Console.WriteLine("Client half: {0}", clientHalf);
                    //Console.WriteLine("Server Half: {0}", serverHalf);

                    int uid = readPacket.ReadInt();

                    if (uid < 0)
                    {
                        PacketHandler.SendPacket(connection, new SendLoginResponse(LoginResponse.SessionRejected));
                        connection.Dispose();
                        break;
                    }
                    string username = readPacket.ReadString();
                    string password = readPacket.ReadString();

                    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                    {
                        PacketHandler.SendPacket(connection, new SendLoginResponse(LoginResponse.InvalidCredentials));
                        connection.Dispose();
                        break;
                    }
                    string hashedPassword = Hashing.HashPassword(password);
                    player = new(new(uid, username, hashedPassword), connection);

                    Console.WriteLine("Credentials: [{0}][{1}][{2}]", username, password, hashedPassword);

                    if (SaveGameHandler.SaveGameExists(username))
                    { // Check if a save game exists for the username
                        if (!SaveGameHandler.LoadSaveGame(player))
                        { // Try to load the save game
                            PacketHandler.SendPacket(connection, new SendLoginResponse(LoginResponse.CouldNotCompleteLogin));
                            player = null;
                            connection.Dispose();
                            break;
                        }
                        if (!SaveGameHandler.VerifyPassword(player.Credentials.Password, hashedPassword))
                        { // Verify whether the given password matches the one from the save game
                            PacketHandler.SendPacket(connection, new SendLoginResponse(LoginResponse.InvalidCredentials));
                            player = null;
                            connection.Dispose();
                            break;
                        }
                        //TODO:
                        //If banned/strikes/temp bans....
                    }
                    else
                    {
                        SaveGameHandler.SaveGame(player); // Save the game for the new player
                    }
                    connection.Player = player;
                    connection.ConnectionState = ConnectionState.Authenticated;
                    // Send a successful response to the client
                    PacketHandler.SendPacket(connection, new SendLoginResponse(LoginResponse.Successful, player.Rights, player.Flagged));
                    break;
            }
        }

    }
}
