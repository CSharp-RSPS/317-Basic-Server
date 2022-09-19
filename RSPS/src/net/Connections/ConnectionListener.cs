using RSPS.src.entity.player;
using RSPS.src.net.Authentication;
using RSPS.src.net.Codec;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace RSPS.src.net.Connections
{
    /// <summary>
    /// Handles client connections
    /// </summary>
    public class ConnectionListener : INetworkListener
    {

        /// <summary>
        /// Holds the players pending login
        /// </summary>
        public readonly Queue<Player> PendingLogin = new();

        /// <summary>
        /// Whether the listener socket is active
        /// </summary>
        public bool IsActive => _listenerSocket != null && _listenerSocket.IsBound;

        /// <summary>
        /// The listener socket
        /// </summary>
        private Socket? _listenerSocket;

        /// <summary>
        /// Represents a callback event handler for a new authenticated player
        /// </summary>
        /// <param name="player">The player</param>
        public delegate void NewPlayerAuthenticated(Player player);

        /// <summary>
        /// The player authenticated event
        /// </summary>
        public event NewPlayerAuthenticated? PlayerAuthenticated;


        public bool Start(NetEndpoint endpoint)
        {
            try
            {
                _listenerSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _listenerSocket.Bind(endpoint.IPEndpoint);
                _listenerSocket.Listen(25);
                _listenerSocket.Blocking = false;
                _listenerSocket.DontFragment = true;
                _listenerSocket.NoDelay = true;

                _listenerSocket.BeginAccept(new AsyncCallback(AcceptCallback), _listenerSocket);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            _listenerSocket?.Dispose();

            PendingLogin.Clear();
        }

        /// <summary>
        /// Attempts to accept a new incoming connection
        /// </summary>
        /// <param name="result"></param>
        public void AcceptCallback(IAsyncResult result)
        {
            if (!IsActive || result.AsyncState == null)
            {
                Console.Error.WriteLine("Invalid connection accept callback");
                return;
            }
            try
            {
                // Get the socket that handles the client request.
                Socket clientSocket = ((Socket)result.AsyncState);
                clientSocket = clientSocket.EndAccept(result);
                // Instantiate the new connection to accept
                Connection newConnection = new(clientSocket);
                // Start receiving data from the new connection and handle it's acceptation
                clientSocket.BeginReceive(newConnection.Buffer, 0, newConnection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadConnectionRequestProtocolCallback), newConnection);
                // Reset the listener socket state to listen for new connection attempts
                _listenerSocket?.BeginAccept(new AsyncCallback(AcceptCallback), _listenerSocket);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Accept callback died");
                Environment.Exit(1);
            }
        }

        private bool ReadProtocolCallback(IAsyncResult result, IProtocolDecoder decoder, Connection connection)
        {
            if (!IsActive || result == null || result.AsyncState == null)
            {
                Console.Error.WriteLine("Invalid read protocol callback");
                connection.Dispose();
                return false;
            }
            int bytesRead = 0;

            try
            {
                bytesRead = connection.ClientSocket.EndReceive(result);
            }
            catch (SocketException ex)
            { // Client likely disconnected from the server
                Debug.WriteLine(ex);
                connection.Dispose();
                return false;
            }
            catch (ObjectDisposedException ex)
            {
                Debug.WriteLine(ex);
                connection.MarkDisconnected();
                return false;
            }
            if (bytesRead <= 0)
            {
                return false;
            }
            byte[]? payload = null;

            using (MemoryStream ms = new(bytesRead))
            { // Read the payload
                ms.Write(connection.Buffer, 0, bytesRead);
                payload = ms.ToArray();
            }
            PacketReader reader = new(payload);

            if (!decoder.Decode(connection, reader))
            {
                connection.Dispose();
                return false;
            }
            connection.ResetBuffer();
            return true;
        }

        private void ReadConnectionRequestProtocolCallback(IAsyncResult result)
        {
            if (result.AsyncState == null)
            {
                return;
            }
            Connection connection = (Connection)result.AsyncState;

            if (!ReadProtocolCallback(result, new ConnectionRequestProtocolDecoder(), connection))
            {
                return;
            }
            connection?.ClientSocket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadLoginProtocolCallback), connection);
        }

        private void ReadLoginProtocolCallback(IAsyncResult result)
        {
            if (result.AsyncState == null)
            {
                return;
            }
            Connection connection = (Connection)result.AsyncState;

            LoginProtocolDecoder loginDecoder = new();
            loginDecoder.AuthenticationFinished += OnAuthenticationFinished;

            if (!ReadProtocolCallback(result, loginDecoder, connection))
            {
                return;
            }
            /*connection?.ClientSocket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadProtocolCallback), connection);*/
        }

        /// <summary>
        /// Handles a finished authentication attempt.
        /// </summary>
        /// <param name="connection">The connection</param>
        /// <param name="player">The authenticated player if any</param>
        private void OnAuthenticationFinished(Player player)
        {
            PlayerAuthenticated?.Invoke(player);

            StartListenForPackets(player);
        }

        private void StartListenForPackets(Player player)
        {
            player.PlayerConnection?.ClientSocket.BeginReceive(player.PlayerConnection.Buffer, 0, player.PlayerConnection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadProtocolCallback), player);
        }

        private void ReadProtocolCallback(IAsyncResult result)
        {
            if (result.AsyncState == null)
            {
                return;
            }
            Player player = (Player)result.AsyncState;

            if (!ReadProtocolCallback(result, new ProtocolDecoder(player), player.PlayerConnection))
            {
                //StartListenForPackets(player);
                return;
            }
            StartListenForPackets(player);
        }

    }
}
