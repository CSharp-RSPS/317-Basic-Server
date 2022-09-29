using RSPS.src.entity.Mobiles.Players;
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
using System.Numerics;
using System.Reflection.PortableExecutable;
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
                // Accept a connection request from a client
                clientSocket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadConnectionRequestCallback), connection);
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
        /// Handles a connection request callback
        /// </summary>
        /// <param name="result">The async operation result</param>
        private void ReadConnectionRequestCallback(IAsyncResult result)
        {
            if (result.AsyncState == null)
            {
                Console.Error.WriteLine("No state passed when handling ReadConnectionRequestCallback");
                return;
            }
            Connection connection = (Connection)result.AsyncState;

            if (HandleProtocolDecoding(result, new ConnectionRequestDecoder(), connection))
            {
                connection.ClientSocket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadLoginCallback), connection);
            }
        }

        /// <summary>
        /// Handles a login callback
        /// </summary>
        /// <param name="result">The async operation result</param>
        private void ReadLoginCallback(IAsyncResult result)
        {
            if (result.AsyncState == null)
            {
                Console.Error.WriteLine("No state passed when handling ReadLoginCallback");
                return;
            }
            Connection connection = (Connection)result.AsyncState;

            LoginDecoder decoder = new();
            decoder.Authenticated += OnAuthenticated;

            HandleProtocolDecoding(result, decoder, connection);
        }

        /// <summary>
        /// Called when a player was authenticated through the login decoder
        /// </summary>
        /// <param name="player">The player</param>
        private void OnAuthenticated(Player player)
        {
            // Asign a packet decoder for the player to their connection
            player.PlayerConnection.PacketDecoder = new PacketDecoder(player);
            //Start listening for packets
            player.PlayerConnection.ClientSocket.BeginReceive(player.PlayerConnection.Buffer, 0, player.PlayerConnection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadPacketCallback), player);
        }

        /// <summary>
        /// Handles a packet callback
        /// </summary>
        /// <param name="result">The async operation result</param>
        private void ReadPacketCallback(IAsyncResult result)
        {
            if (result.AsyncState == null)
            {
                Console.Error.WriteLine("No state passed when handling ReadPacketCallback");
                return;
            }
            Player player = (Player)result.AsyncState;

            if (player.PlayerConnection.PacketDecoder == null)
            {
                Console.Error.WriteLine("No packetdecoder found on connection when handling ReadPacketCallback");
                return;
            }
            if (HandleProtocolDecoding(result, player.PlayerConnection.PacketDecoder, player.PlayerConnection))
            {
                player.PlayerConnection.ClientSocket.BeginReceive(player.PlayerConnection.Buffer, 0, player.PlayerConnection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadPacketCallback), player);
            }
            //TODO work with connection, player stats in decoder player.PlayerConnection.PacketDecoder
        }

        private bool HandleProtocolDecoding(IAsyncResult result, IProtocolDecoder decoder, Connection connection)
        {
            if (connection.ConnectionState == ConnectionState.Disconnected)
            {
                connection.Dispose();
                return false;
            }
            try
            {
                // Retrieve the bytes received from the client
                int bytesRead = connection.ClientSocket.EndReceive(result);

                if (bytesRead <= 0)
                {
                    return true;
                } 
                // Write the received bytes to a new buffer
                byte[] packetBuffer = new byte[bytesRead];
                Array.Copy(connection.Buffer, 0, packetBuffer, 0, bytesRead);
                // Wrap the buffer into a packet reader and decode the packet(s)
                if (!decoder.Decode(connection, new(packetBuffer)))
                {
                    Console.Error.WriteLine("Decoding failed using decoder: {0}", decoder.GetType().Name);
                    connection.Dispose();
                    return false;
                }
                // Resets the connection buffer for the next packet
                connection.ResetBuffer();
                return true;
            }
            catch (SocketException ex)
            { // Client likely disconnected from the server
                Debug.WriteLine(ex);
                connection.Dispose();
                return false;
            }
            catch (ObjectDisposedException ex)
            { // Socket is disposed
                Debug.WriteLine(ex);
                connection.MarkDisconnected();
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                connection.Dispose();
                return false;
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
