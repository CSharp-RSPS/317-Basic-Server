using RSPS.src.entity.player;
using RSPS.src.net.Authentication;
using RSPS.src.net.packet;
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
        /// Holds the active connections
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

        public void AcceptCallback(IAsyncResult result)
        {
            if (!IsActive || result.AsyncState == null)
            {
                return;
            }
            try
            {
                // Get the socket that handles the client request.  
                Socket clientSocket = ((Socket)result.AsyncState).EndAccept(result);
                Connection connection = new(clientSocket);
                // Start receicing data on the new client connection
                connection.ClientSocket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadCallback), connection);
                //Start accepting a new connection. Keep the listeners off the main game loop
                _listenerSocket?.BeginAccept(new AsyncCallback(AcceptCallback), _listenerSocket);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Accept callback died");
                Environment.Exit(1);
            }
        }

        public void ReadCallback(IAsyncResult result)
        {
            if (_listenerSocket == null || result.AsyncState == null)
            {
                return;
            }
            try
            {
                // Retrieve the state object and the handler socket  
                // from the asynchronous state object.  
                Connection connection = (Connection)result.AsyncState;
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
                    return;
                }
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
                PacketReader pr = new(payload);

                while (pr.PayloadPosition < pr.Payload.Length)
                { // Handle the received packet
                    if (connection.ConnectionState < ConnectionState.Authenticated)
                    { // Handle a new connection
                        LoginProtocolDecoder.DecodeLogin(connection, out Player? player);
                        //RSALoginProtocolDecoder.DecodeLogin(connection); - for new client

                        if (connection.ConnectionState != ConnectionState.Authenticated)
                        {
                            break;
                        }
                        if (player == null)
                        {
                            Console.Error.WriteLine("Failed to authenticate player");
                            break;
                        }
                        // Enqueue the player to perform a proper login to the game world
                        PendingLogin.Enqueue(connection.Player);
                        break;
                    }
                    // Handle a packet for an existing connection
                    int packetOpCode = pr.ReadByte() & 0xFF;
                    packetOpCode = packetOpCode - connection.NetworkDecryptor.getNextValue() & 0xFF;// -- cryption
                    //Console.WriteLine("packet op code: " + packetOpCode);
                    int packetLength = PACKET_LENGTHS[packetOpCode];

                    if (packetLength == -1)//variable length packet
                    {
                        if (pr.PayloadPosition >= pr.Payload.Length)
                        {
                            break;
                        }
                        packetLength = pr.ReadByte();
                        packetLength = packetLength & 0xFF;//new
                    }
                    if (pr.Payload.Length >= packetLength)
                    {
                        PacketHandler.HandlePacket(connection, packetOpCode, packetLength, pr);
                    }
                }
                // Reset the connection data buffer
                connection.Buffer = new byte[Constants.BufferSize];
                // Start listening for the next packet
                connection.ClientSocket.BeginReceive(connection.Buffer, 0, connection.Buffer.Length, SocketFlags.None,
                        new AsyncCallback(ReadCallback), connection);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Network listener can't receive packet");
                Console.Error.WriteLine(ex);
                Dispose();
            }
        }

        /**
         * Lengths for the various packets.
         */
        public static readonly int[] PACKET_LENGTHS = {
            0, 0, 0, 1, -1, 0, 0, 0, 0, 0, // 0
            0, 0, 0, 0, 8, 0, 6, 2, 2, 0, // 10
            0, 2, 0, 6, 0, 12, 0, 0, 0, 0, // 20
            0, 0, 0, 0, 0, 8, 4, 0, 0, 2, // 30
            2, 6, 0, 6, 0, -1, 0, 0, 0, 0, // 40
            0, 0, 0, 12, 0, 0, 0, 0, 8, 0, // 50
            0, 8, 0, 0, 0, 0, 0, 0, 0, 0, // 60
            6, 0, 2, 2, 8, 6, 0, -1, 0, 6, // 70
            0, 0, 0, 0, 0, 1, 4, 6, 0, 0, // 80
            0, 0, 0, 0, 0, 3, 0, 0, -1, 0, // 90
            0, 13, 0, -1, 0, 0, 0, 0, 0, 0,// 100
            0, 0, 0, 0, 0, 0, 0, 6, 0, 0, // 110
            1, 0, 6, 0, 0, 0, -1, 0, 2, 6, // 120
            0, 4, 6, 8, 0, 6, 0, 0, 0, 2, // 130
            0, 0, 0, 0, 0, 6, 0, 0, 0, 0, // 140
            0, 0, 1, 2, 0, 2, 6, 0, 0, 0, // 150
            0, 0, 0, 0, - 1, -1, 0, 0, 0, 0,// 160
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 170
            0, 8, 0, 3, 0, 2, 0, 0, 8, 1, // 180
            0, 0, 12, 0, 0, 0, 0, 0, 0, 0, // 190
            2, 0, 0, 0, 0, 0, 0, 0, 4, 0, // 200
            4, 0, 0, 0, 7, 8, 0, 0, 10, 0, // 210
            0, 0, 0, 0, 0, 0, -1, 0, 6, 0, // 220
            1, 0, 0, 0, 6, 0, 6, 8, 1, 0, // 230
            0, 4, 0, 0, 0, 0, -1, 0, -1, 4,// 240
            0, 0, 6, 6, 0, 0, 0 // 250
        };

    }
}
