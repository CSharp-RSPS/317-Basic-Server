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
using System.Text;

namespace RSPS.src.net.Connections
{
    /// <summary>
    /// Handles client connections
    /// </summary>
    public class ConnectionListener : INetworkListener
    {

        /// <summary>
        /// Holds the connections connected to the listener
        /// </summary>
        public readonly List<Connection> Connections = new();

        /// <summary>
        /// Whether the listener socket is active
        /// </summary>
        public bool IsActive => _listenerSocket != null && _listenerSocket.IsBound;

        /// <summary>
        /// The listener socket
        /// </summary>
        private Socket? _listenerSocket;

        /// <summary>
        /// Represents a player authentication
        /// </summary>
        /// <param name="player">The player</param>
        public delegate void PlayerAuthentication(Player player);

        /// <summary>
        /// Indiciates a player authenticated
        /// </summary>
        public event PlayerAuthentication? PlayerAuthenticated;


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

            // Dispose and clear any connections made with the listener
            Connections.ForEach(c => c.Dispose());
            Connections.Clear();

            try
            {
                _listenerSocket?.Dispose();
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Listener socket already disposed");
            }
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
                newConnection.Disconnected += OnDisconnected;
                newConnection.Authenticated += OnAuthenticated;
                Connections.Add(newConnection);
                // Start listening for incoming packets on the client socket
                clientSocket.BeginReceive(newConnection.Buffer, 0, newConnection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadProtocolCallback), newConnection);
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

        /// <summary>
        /// Indicates a connection disconnected
        /// </summary>
        /// <param name="connection">The connection</param>
        private void OnDisconnected(Connection connection)
        {
            Connections.Remove(connection);
        }

        /// <summary>
        /// Indiciates a player authenticated through the connection
        /// </summary>
        /// <param name="player">The player</param>
        private void OnAuthenticated(Player player)
        {
            PlayerAuthenticated?.Invoke(player);
        }

        /// <summary>
        /// Reads a protocol callback
        /// </summary>
        /// <param name="result">The callback state object</param>
        private void ReadProtocolCallback(IAsyncResult result)
        {
            if (!IsActive || result == null || result.AsyncState == null)
            {
                Console.Error.WriteLine("Invalid read protocol callback");
                return;
            }
            Connection connection = (Connection)result.AsyncState;

            if (connection.ConnectionState == ConnectionState.Disconnected)
            {
                connection.Dispose();
                return;
            }
            if (connection.ProtocolDecoder == null)
            {
                Console.Error.WriteLine("Connection has no protocol decoder assigned");
                connection.Dispose();
                return;
            }
            try
            {
                int bytesRead = connection.ClientSocket.EndReceive(result);

                if (bytesRead <= 0)
                {
                    return;
                }
                byte[]? payload = null;

                using (MemoryStream ms = new(bytesRead))
                { // Read the payload
                    ms.Write(connection.Buffer, 0, bytesRead);
                    payload = ms.ToArray();
                }
                IProtocolDecoder? nextDecoder = connection.ProtocolDecoder.Decode(connection, new(payload));

                if (nextDecoder == null)
                {
                    Console.Error.WriteLine("Decoding failed using decoder: {0}", connection.ProtocolDecoder.GetType().Name);
                    connection.Dispose();
                    return;
                }
                // Assign the new decoder to use
                connection.ProtocolDecoder = nextDecoder;
                // Reset the payload buffer
                connection.ResetBuffer();
                // Start listening for the next incoming packet
                connection.ClientSocket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadProtocolCallback), connection);
            }
            catch (SocketException ex)
            { // Client likely disconnected from the server
                Debug.WriteLine(ex);
                connection.Dispose();
                return;
            }
            catch (ObjectDisposedException ex)
            {
                Debug.WriteLine(ex);
                connection.MarkDisconnected();
                return;
            }
        }

    }
}
