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
        /// The endpoint
        /// </summary>
        public string Endpoint { get; }

        /// <summary>
        /// The port
        /// </summary>
        public int Port { get; }

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
        public event NewPlayerAuthenticated PlayerAuthenticated;


        /// <summary>
        /// Creates a new connection listener
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="port"></param>
        public ConnectionListener(string endpoint, int port)
        {
            Endpoint = endpoint;
            Port = port;
        }

        public bool Start()
        {
            IPEndPoint endPoint = new(IPAddress.Parse(Endpoint), Port);

            try
            {
                _listenerSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _listenerSocket.Bind(endPoint);
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
                Socket clientSocket = ((Socket)result.AsyncState).EndAccept(result);
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

        private bool ReadProtocolCallback(IAsyncResult result, IProtocolDecoder decoder, out Connection? connection)
        {
            if (!IsActive || result.AsyncState == null)
            {
                Console.Error.WriteLine("Invalid read protocol callback");
                connection = null;
                return false;
            }
            connection = (Connection)result.AsyncState;

            int bytesRead = 0;

            try
            {
                bytesRead = connection.ClientSocket.EndReceive(result);
            }
            catch (SocketException ex)
            { // Client likely disconnected from the server
                Console.Error.WriteLine("Client disconnected forcefully");
                //Console.Error.WriteLine(ex);
                connection.Dispose();
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
            // Reset the connection data buffer
            connection.Buffer = new byte[Constants.BufferSize];
            return true;
        }

        private void ReadConnectionRequestProtocolCallback(IAsyncResult result)
        {
            if (!ReadProtocolCallback(result, new ConnectionRequestProtocolDecoder(), out Connection? connection))
            {
                return;
            }
            connection?.ClientSocket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadLoginProtocolCallback), connection);
        }

        private void ReadLoginProtocolCallback(IAsyncResult result)
        {
            LoginProtocolDecoder loginDecoder = new();
            loginDecoder.AuthenticationFinished += OnAuthenticationFinished;

            if (!ReadProtocolCallback(result, loginDecoder, out Connection? connection))
            {
                return;
            }
            connection?.ClientSocket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadProtocolCallback), connection);
        }

        /// <summary>
        /// Handles a finished authentication attempt.
        /// </summary>
        /// <param name="connection">The connection</param>
        /// <param name="player">The authenticated player if any</param>
        /// <param name="authenticationResponse">The authentication response</param>
        private void OnAuthenticationFinished(Connection connection, Player? player, AuthenticationResponse authenticationResponse)
        {
            PacketHandler.SendPacket(connection, new SendLoginResponse(authenticationResponse,
                player == null ? 0 : player.Rights, 
                player != null && player.Flagged));

            if (player == null)
            {
                return;
            }
            PlayerAuthenticated(player);
        }

        private void ReadProtocolCallback(IAsyncResult result)
        {
            if (!ReadProtocolCallback(result, new ProtocolDecoder(), out Connection? connection))
            {
                return;
            }
            connection?.ClientSocket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None,
                     new AsyncCallback(ReadProtocolCallback), connection);
        }

    }
}
