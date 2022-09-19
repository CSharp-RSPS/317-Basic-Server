using RSPS.src.entity.player;
using RSPS.src.net.Authentication;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.Codec
{
    public class LoginProtocolDecoder : IProtocolDecoder
    {


        public IProtocolDecoder? Decode(Connection connection, PacketReader reader)
        {
            int connectionType = reader.ReadByte();

            if (connectionType != 16 && connectionType != 18)
            { // 16 = new login, 18 = reconnection
                PacketHandler.SendPacket(connection, new SendLoginResponse(AuthenticationResponse.Unknown));
                return null;
            }
            int blockLength = reader.ReadByte();//loginSize = 76

            if (reader.Payload.Length/*connection.Buffer.Length*/ < blockLength)
            {
                Console.WriteLine("State buffer length is less than block length");
                PacketHandler.SendPacket(connection, new SendLoginResponse(AuthenticationResponse.Unknown));
                return null;
            }
            if (reader.ReadByte() != 255)
            { // Magic number
                PacketHandler.SendPacket(connection, new SendLoginResponse(AuthenticationResponse.Unknown));
                return null;
            }
            if (reader.ReadShort() != 317)
            { // Client version
                PacketHandler.SendPacket(connection, new SendLoginResponse(AuthenticationResponse.GameWasUpdated));
                return null;
            }
            reader.ReadByte();//High/low memory version

            // Skip the CRC keys.
            for (int i = 0; i < 9; i++)
            {
                reader.ReadInt();
            }
            int RSABlock = reader.ReadByte(); //Skip RSA block length

            if (reader.ReadByte() != 10) 
            { // RSA Opcode
                PacketHandler.SendPacket(connection, new SendLoginResponse(AuthenticationResponse.SessionRejected));
                return null;
            }
            // Set up the ISAAC ciphers.
            long clientHalf = reader.ReadLong(); // Client session key
            long serverHalf = reader.ReadLong(); // Server session key

            int[] isaacSeed = { (int)(clientHalf >> 32), (int)clientHalf, (int)(serverHalf >> 32), (int)serverHalf };

            ISAACCipher decryptor = new(isaacSeed);

            for (int i = 0; i < 4; i++)
            {
                isaacSeed[i] += 50;
            }
            ISAACCipher encryptor = new(isaacSeed);

            int uid = reader.ReadInt();

            string username = reader.ReadString();
            string password = reader.ReadString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                PacketHandler.SendPacket(connection, new SendLoginResponse(AuthenticationResponse.InvalidCredentials));
                return null;
            }
            string hashedPassword = Hashing.HashPassword(password);
            Player player = new(new(uid, username, hashedPassword), connection);

            Console.WriteLine("Credentials: [{0}][{1}]", username, password);

            if (SaveGameHandler.SaveGameExists(username))
            { // Check if a save game exists for the username
                if (!SaveGameHandler.LoadSaveGame(player))
                { // Try to load the save game
                    PacketHandler.SendPacket(connection, new SendLoginResponse(AuthenticationResponse.CouldNotCompleteLogin));
                    return null;
                }
                if (!SaveGameHandler.VerifyPassword(player.Credentials.Password, hashedPassword))
                { // Verify whether the given password matches the one from the save game
                    PacketHandler.SendPacket(connection, new SendLoginResponse(AuthenticationResponse.InvalidCredentials));
                    return null;
                }
                //TODO:
                //If banned/strikes/temp bans....
            }
            else
            {
                SaveGameHandler.SaveGame(player); // Save the game for the new player
            }
            connection.NetworkDecryptor = decryptor;
            connection.NetworkEncryptor = encryptor;

            PacketHandler.SendPacket(connection, new SendLoginResponse(connectionType == 16 
                ? AuthenticationResponse.Successful 
                : AuthenticationResponse.SuccessfulReconnect));

            connection.PlayerAuthenticated(player);

            return new ProtocolDecoder(player);
        }

    }
}
