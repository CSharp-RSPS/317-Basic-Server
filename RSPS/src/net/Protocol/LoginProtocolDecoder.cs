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

        /// <summary>
        /// Represents a callback event handler for an authentication attempt for a player
        /// </summary>
        /// <param name="player">The player</param>
        public delegate void FinishAuthentication(Player player);

        /// <summary>
        /// The authentifcation finished event
        /// </summary>
        public event FinishAuthentication? AuthenticationFinished;


        public bool Decode(Connection connection, PacketReader reader)
        {
            int connectionType = reader.ReadByte();

            if (connectionType != 16 && connectionType != 18)
            { // 16 = new login, 18 = reconnection
                PacketHandler.SendPacket(connection, new SendLoginResponse(AuthenticationResponse.Unknown));
                return false;
            }
            int blockLength = reader.ReadByte();//loginSize = 76

            if (reader.Payload.Length/*connection.Buffer.Length*/ < blockLength)
            {
                Console.WriteLine("State buffer length is less than block length");
                PacketHandler.SendPacket(connection, new SendLoginResponse(AuthenticationResponse.Unknown));
                return false;
            }
            if (reader.ReadByte() != 255)
            { // Magic number
                PacketHandler.SendPacket(connection, new SendLoginResponse(AuthenticationResponse.Unknown));
                return false;
            }
            if (reader.ReadShort() != 317)
            { // Client version
                PacketHandler.SendPacket(connection, new SendLoginResponse(AuthenticationResponse.GameWasUpdated));
                return false;
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
                return false;
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
                return false;
            }
            string hashedPassword = Hashing.HashPassword(password);
            Player player = new(new(uid, username, hashedPassword), connection);

            Console.WriteLine("Credentials: [{0}][{1}][{2}]", username, password, hashedPassword);

            if (SaveGameHandler.SaveGameExists(username))
            { // Check if a save game exists for the username
                if (!SaveGameHandler.LoadSaveGame(player))
                { // Try to load the save game
                    PacketHandler.SendPacket(connection, new SendLoginResponse(AuthenticationResponse.CouldNotCompleteLogin));
                    return false;
                }
                if (!SaveGameHandler.VerifyPassword(player.Credentials.Password, hashedPassword))
                { // Verify whether the given password matches the one from the save game
                    PacketHandler.SendPacket(connection, new SendLoginResponse(AuthenticationResponse.InvalidCredentials));
                    return false;
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
            connection.ConnectionState = ConnectionState.Authenticated;

           // connection.Player = player;

            PacketHandler.SendPacket(connection, new SendLoginResponse(connectionType == 16 
                ? AuthenticationResponse.Successful 
                : AuthenticationResponse.SuccessfulReconnect));

            AuthenticationFinished?.Invoke(player);

            return true;
        }

    }
}
