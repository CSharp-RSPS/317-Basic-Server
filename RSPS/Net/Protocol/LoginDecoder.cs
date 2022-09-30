using RSPS.Entities.Mobiles.Players;
using RSPS.Net.Authentication;
using RSPS.Net.Ciphers;
using RSPS.Net.Connections;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Security;
using RSPS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.Codec
{
    /// <summary>
    /// Represents a login request decoder based on the RS2 protocol
    /// </summary>
    public sealed class LoginDecoder : IProtocolDecoder
    {

        /// <summary>
        /// Indiciates authentication completed
        /// </summary>
        /// <param name="player">The player</param>
        public delegate void AuthenticationComplete(Player player);

        /// <summary>
        /// The authentication complete event
        /// </summary>
        public event AuthenticationComplete? Authenticated;

        public bool Decode(Connection connection, PacketReader reader)
        {
            int connectionType = reader.ReadByte(false);

            if (connectionType != 16 && connectionType != 18)
            { // 16 = new login, 18 = reconnection
                SendLoginResponse(connection, AuthenticationResponse.Unknown);
                return false;
            }
            int loginBlockSize = reader.ReadByte(false); //loginSize = 76
            int encryptedBlockSize = loginBlockSize - (0x24 + 0x1 + 0x1 + 0x2);

            if (encryptedBlockSize < 1)
            {
                Console.WriteLine("Invalid encrypted login block size");
                SendLoginResponse(connection, AuthenticationResponse.Unknown);
                return false;
            }
            if (reader.Buffer.Length/*connection.Buffer.Length*/ < loginBlockSize)
            {
                Console.WriteLine("State buffer length is less than block length");
                SendLoginResponse(connection, AuthenticationResponse.Unknown);
                return false;
            }
            if (reader.ReadByte(false) != 255)
            { // Magic number
                SendLoginResponse(connection, AuthenticationResponse.Unknown);
                return false;
            }
            if (reader.ReadShort() != 317)
            { // Client version
                SendLoginResponse(connection, AuthenticationResponse.GameWasUpdated);
                return false;
            }
            reader.ReadByte(false);//High/low memory version

            // Skip the CRC keys.
            for (int i = 0; i < 9; i++)
            {
                reader.ReadInt();
            }
            int RSABlock = reader.ReadByte(false); //Skip RSA block length

            if (reader.ReadByte(false) != 10)
            { // RSA Opcode
                SendLoginResponse(connection, AuthenticationResponse.SessionRejected);
                return false;
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

            string username = reader.ReadRS2String();
            string password = reader.ReadRS2String();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                SendLoginResponse(connection, AuthenticationResponse.InvalidCredentials);
                return false;
            }
            string hashedPassword = Hashing.HashPassword(password);
            Player player = new(new(uid, username, hashedPassword), connection);

            Console.WriteLine("Credentials: [{0}][{1}]", username, password);

            if (SaveGameHandler.SaveGameExists(username))
            { // Check if a save game exists for the username
                if (!SaveGameHandler.LoadSaveGame(player))
                { // Try to load the save game
                    SendLoginResponse(connection, AuthenticationResponse.CouldNotCompleteLogin);
                    return false;
                }
                if (!SaveGameHandler.VerifyPassword(player.Credentials.Password, hashedPassword))
                { // Verify whether the given password matches the one from the save game
                    SendLoginResponse(connection, AuthenticationResponse.InvalidCredentials);
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

            if (!WorldHandler.World.Online)
            {
                SendLoginResponse(connection, AuthenticationResponse.LoginServerOffline);
                return false;
            }
            if (WorldHandler.World.IsFull)
            {
                SendLoginResponse(connection, AuthenticationResponse.WorldFull);
                return false;
            }
            if (WorldHandler.World.Players.ByUsername(username) != null)
            {
                SendLoginResponse(connection, AuthenticationResponse.AlreadyLoggedIn);
                return false;
            }
            if (string.IsNullOrEmpty(connection.IpAddress))
            {
                SendLoginResponse(connection, AuthenticationResponse.SessionRejected);
                return false;
            }
            if (WorldHandler.World.ConnectionListener.Connections
                .Where(c => c.IpAddress == connection.IpAddress).ToList().Count > Constants.MaxSimultaneousConnections)
            {
                SendLoginResponse(connection, AuthenticationResponse.LoginLimitExceeded);
                return false;
            }
            if (WorldHandler.World.HasUpdatePending)
            {
                SendLoginResponse(connection, AuthenticationResponse.ServerBeingUpdated);
                return false;
            }
            //TODO: login attempts exceeded
            //TODO: standing in members only area
            //TODO: Just left another world, will be transfered in ...

            SendLoginResponse(connection, connectionType == 16
                ? AuthenticationResponse.Successful
                : AuthenticationResponse.SuccessfulReconnect,
                player);

            connection.ConnectionState = ConnectionState.Authenticated;

            WorldHandler.World.Players.Add(player);
            PlayerManager.InitializeSession(player);
            PlayerManager.Login(player);

            Authenticated?.Invoke(player);

            return true;
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
            PacketWriter pw = new(loginResponse == AuthenticationResponse.SuccessfulReconnect ? 1 : 3);

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
