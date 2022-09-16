using RSPS.src.entity.player;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net
{
    public class LoginProtocolDecoder
    {


        public static void DecodeLogin(Connection connection, out string? username, out string? password)
        {
            username = null;
            password = null;

            //Console.WriteLine("Buffer Size: {0}", connection.buffer.Length);
            PacketReader readPacket;
            switch (connection.connectionState)
            {
                case ConnectionState.Handshake://connection state
                    readPacket = Packet.CreatePacketReader(connection.buffer);
                    int opcode = readPacket.ReadByte() & 0xff;
                    int nameHash = readPacket.ReadByte() & 0xff;
                    if (opcode == 14)
                    {
                        //Console.WriteLine("opcode = game");
                        //Console.WriteLine("NameHash: {0}", nameHash);
                        connection.connectionState = ConnectionState.Authenticate;
                        MemoryStream stream = new MemoryStream(17);
                        long dummyData = 0;
                        stream.Write(BitConverter.GetBytes(dummyData));
                        stream.WriteByte(0);
                        stream.Write(BitConverter.GetBytes(new Random().NextInt64()));
                        Program.SendGlobalByes(connection, stream.GetBuffer());
                    }
                    if (opcode == 15)
                    {
                        Console.WriteLine("opcode = update");
                    }
                break;

                case ConnectionState.Authenticate://login state
                    readPacket = Packet.CreatePacketReader(connection.buffer);

                    //Console.WriteLine("We are inside the login state");
                    int loginType = readPacket.ReadByte();
                    //Console.WriteLine("LoginType: {0}", loginType);
                    if (loginType != 16 && loginType != 18)
                    {
                        Console.WriteLine("Invalid login Type: {0}", loginType);
                        connection.clientSocket.Close();
                    }

                    int blockLength = readPacket.ReadByte();//loginSize = 76
                    //Console.WriteLine("Block Length is {0}", blockLength);
                    if (connection.buffer.Length < blockLength)
                    {
                        Console.WriteLine("State buffer length is less than block length");
                    }

                    int magicId = readPacket.ReadByte();//255
                    //Console.WriteLine("Magic ID: {0}", magicId);

                    int clientVersion = readPacket.ReadShort();
                    //Console.WriteLine("Client Version {0}", clientVersion);//needs to be 317. Bitconvert works
                    if (clientVersion != 317)
                    {
                        Console.WriteLine("Incorrect client version");
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
                        connection.clientSocket.Close();
                        return;
                    }

                    // Set up the ISAAC ciphers.
                    long clientHalf = readPacket.ReadLong();
                    //Console.WriteLine("Isaac client: " + clientHalf);
                    long serverHalf = readPacket.ReadLong();
                    //Console.WriteLine("Isaac Server: " + serverHalf);
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
                    //Console.WriteLine("UID: {0}", uid);

                    username = readPacket.ReadString();
                    password = readPacket.ReadString();
                    Console.WriteLine("Username: {0}", username);
                    Console.WriteLine("Password: {0}", password);
                    connection.connectionState = ConnectionState.Authenticated;
                    // send the login response to see if we can login
                break;
            }
        }

    }
}
