using RSPS.src.entity.player;
using RSPS.src.net.Authentication;
using RSPS.src.net.Codec;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Worlds;
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
        /// The max amount of requests the listener socket can listen to before it gives Server busy response
        /// </summary>
        private static readonly int MaxSimultaneousRequests = 25;

        /// <summary>
        /// The network endpoint
        /// </summary>
        public NetEndpoint? Endpoint { get; private set; }

        /// <summary>
        /// The details of the world the connection listener is for
        /// </summary>
        public WorldDetails? Details { get; private set; }

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


        public bool Start(NetEndpoint endpoint, WorldDetails details)
        {
            Details = details;
            Endpoint = endpoint;

            try
            {
                // Setup the listener socket
                _listenerSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) {
                    Blocking = false,
                    DontFragment = true,
                    NoDelay = true
                };
                // Bind the listener socket
                _listenerSocket.Bind(endpoint.IPEndpoint);
                // Start listening to requests
                _listenerSocket.Listen(MaxSimultaneousRequests);
                // Wait for an incoming connection attempt
                _listenerSocket.BeginAccept(new AsyncCallback(AcceptCallback), _listenerSocket);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Attempts to accept a new incoming connection
        /// </summary>
        /// <param name="result"></param>
        public void AcceptCallback(IAsyncResult result)
        {
            if (result.AsyncState == null)
            {
                Console.Error.WriteLine("No state passed with accept callback");
                return;
            }
            if (Details == null)
            {
                Console.Error.WriteLine("No world details known in connectionlistener");
                return;
            }
            try
            {
                // Get the socket that handles the client request.
                Socket clientSocket = ((Socket)result.AsyncState);
                clientSocket = clientSocket.EndAccept(result);
                // Instantiate the new connection to accept
                Connection connection = new(clientSocket, Details);
                connection.Disconnected += OnDisconnected;
                Connections.Add(connection);
                // Start listening for incoming packets on the client socket
                clientSocket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadProtocolCallback), connection);
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
        /// Reads a protocol callback
        /// </summary>
        /// <param name="result">The callback state object</param>
        private void ReadProtocolCallback(IAsyncResult result)
        {
            if (result.AsyncState == null)
            {
                Console.Error.WriteLine("No state passed with protocol callback");
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
                // Retrieve the bytes received from the client
                int bytesRead = connection.ClientSocket.EndReceive(result);

                if (bytesRead <= 0)
                {
                    return;
                }
                if (bytesRead > connection.Buffer.Length)
                {
                    Console.Error.WriteLine("Read {0} bytes but buffer only supports {1}", bytesRead, connection.Buffer.Length);
                    return;
                }
                byte[]? buffer = null;

                using (MemoryStream ms = new(bytesRead))
                { // Write the received data to a byte buffer through a memory stream
                    ms.Write(connection.Buffer, 0, bytesRead);
                    buffer = ms.ToArray();
                }
                IProtocolDecoder? nextDecoder = connection.ProtocolDecoder.Decode(connection, new(buffer));

                if (nextDecoder == null)
                {
                    Console.Error.WriteLine("Decoding failed using decoder: {0}", connection.ProtocolDecoder.GetType().Name);
                    connection.Dispose();
                    return;
                }
                // Set the next protocol decoder
                connection.ProtocolDecoder = nextDecoder;
                // Reset the data buffer
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            // Dispose and clear any connections made with the listener
            Connections.ForEach(c => c.Dispose());
            Connections.Clear();

            try
            {
                _listenerSocket?.Shutdown(SocketShutdown.Both);
                _listenerSocket?.Close();
                _listenerSocket?.Dispose();
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("Listener socket already disposed");
            }
        }

    }
}
