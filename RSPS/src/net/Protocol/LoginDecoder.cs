using RSPS.src.entity.player;
using RSPS.src.net.Authentication;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.Codec
{
    /// <summary>
    /// Represents a login request decoder based on the RS2 protocol
    /// </summary>
    public sealed class LoginDecoder : IProtocolDecoder
    {


        public IProtocolDecoder? Decode(Connection connection, PacketReader reader)
        {
            int connectionType = reader.ReadByte();

            if (connectionType != 16 && connectionType != 18)
            { // 16 = new login, 18 = reconnection
                SendLoginResponse(connection, AuthenticationResponse.Unknown);
                return null;
            }
            int loginBlockSize = reader.ReadByte(); //loginSize = 76
            int encryptedBlockSize = loginBlockSize - (0x24 + 0x1 + 0x1 + 0x2);

            if (encryptedBlockSize < 1)
            {
                Console.WriteLine("Invalid encrypted login block size");
                SendLoginResponse(connection, AuthenticationResponse.Unknown);
                return null;
            }
            if (reader.Data.Length/*connection.Buffer.Length*/ < loginBlockSize)
            {
                Console.WriteLine("State buffer length is less than block length");
                SendLoginResponse(connection, AuthenticationResponse.Unknown);
                return null;
            }
            if (reader.ReadByte() != 255)
            { // Magic number
                SendLoginResponse(connection, AuthenticationResponse.Unknown);
                return null;
            }
            if (reader.ReadShort() != 317)
            { // Client version
                SendLoginResponse(connection, AuthenticationResponse.GameWasUpdated);
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
                SendLoginResponse(connection, AuthenticationResponse.SessionRejected);
                return null;
            }
            // Set up the ISAAC ciphers.
            long clientHalf = reader.ReadLong(); // Client session key
            long serverHalf = reader.ReadLong(); // Server session key

            int[] isaacSeed = {
                (int)(clientHalf >> 32),
                (int)clientHalf,
                (int)(serverHalf >> 32),
                (int)serverHalf
            };
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
                SendLoginResponse(connection, AuthenticationResponse.InvalidCredentials);
                return null;
            }
            string hashedPassword = Hashing.HashPassword(password);
            Player player = new(new(uid, username, hashedPassword), connection);

            Console.WriteLine("Credentials: [{0}][{1}]", username, password);

            if (SaveGameHandler.SaveGameExists(username))
            { // Check if a save game exists for the username
                if (!SaveGameHandler.LoadSaveGame(player))
                { // Try to load the save game
                    SendLoginResponse(connection, AuthenticationResponse.CouldNotCompleteLogin);
                    return null;
                }
                if (!SaveGameHandler.VerifyPassword(player.Credentials.Password, hashedPassword))
                { // Verify whether the given password matches the one from the save game
                    SendLoginResponse(connection, AuthenticationResponse.InvalidCredentials);
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

            World? world = WorldHandler.ResolveWorld(connection);

            if (world == null)
            {
                SendLoginResponse(connection, AuthenticationResponse.InvalidLoginServer);
                return null;
            }
            if (!world.Online)
            {
                SendLoginResponse(connection, AuthenticationResponse.LoginServerOffline);
                return null;
            }
            if (world.IsFull)
            {
                SendLoginResponse(connection, AuthenticationResponse.WorldFull);
                return null;
            }
            if (WorldHandler.FindPlayerByUsername(username) != null)
            {
                SendLoginResponse(connection, AuthenticationResponse.AlreadyLoggedIn);
                return null;
            }
            int loginsFromSameIp = world.ConnectionListener.Connections.Where(c => c.IpAddress == connection.IpAddress).ToList().Count;

            if (world.ConnectionListener.Connections.Where(c => c.IpAddress == connection.IpAddress).ToList().Count > Constants.MaxSimultaneousConnections)
            {
                SendLoginResponse(connection, AuthenticationResponse.LoginLimitExceeded);
                return null;
            }
            //TODO: world being updated
            //TODO: login attempts exceeded
            //TODO: standing in members only area
            //TODO: Just left another world, will be transfered in ...

            SendLoginResponse(connection, connectionType == 16
                ? AuthenticationResponse.Successful
                : AuthenticationResponse.SuccessfulReconnect,
                player);

            PlayerManager.InitializeSession(player);
            PlayerManager.Login(player, connection.WorldDetails);
            world.Players.PendingLogin.Enqueue(player);

            return new PacketDecoder(player);
        }

        /// <summary>
        /// Sends a login response to the client
        /// </summary>
        /// <param name="loginResponse">The login response</param>
        /// <param name="player">The player if any was instantiated</param>
        private static void SendLoginResponse(Connection connection, AuthenticationResponse loginResponse, Player? player = null)
        {
            if ((loginResponse == AuthenticationResponse.Successful || loginResponse == AuthenticationResponse.SuccessfulReconnect)
                && player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }
           /* using (MemoryStream ms = new MemoryStream(3))
            {
                ms.WriteByte(2);//response all is good
                ms.WriteByte(1);//player rights
                ms.WriteByte(0);//not clue. comment out when working with new client
                connection.Send(ms.ToArray());

                //Program.SendGlobalByes(PlayerConnection, loginResponse.GetBuffer());
            }
                */
            
            PacketWriter pw = Packet.CreatePacketWriter(loginResponse == AuthenticationResponse.SuccessfulReconnect ? 1 : 3);

            pw.WriteByte((int)loginResponse);

            if (loginResponse != AuthenticationResponse.SuccessfulReconnect)
            { // We only need to send these when it's a new login attempt
                pw.WriteByte(player == null ? 0 : (int)player.Rights);
                pw.WriteByte(player == null ? 0 : (player.Flagged ? 1 : 0)); //1 = flagged (information about mouse movements etc. are sent to the server. Suspected bot accounts are flagged.)
            }
            connection.Send(pw);
        }

    }
}
