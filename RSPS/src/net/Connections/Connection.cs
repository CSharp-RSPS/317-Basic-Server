using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.Authentication;
using RSPS.src.net.Codec;
using RSPS.src.net.packet;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.Connections
{
    /// <summary>
    /// Represents a client connection to the server.
    /// </summary>
    public class Connection : IDisposable
    {

        /// <summary>
        /// The max. size of the data buffer
        /// </summary>
        private static readonly int MaxBufferSize = 4096;

        /// <summary>
        /// The packet decoder to be used by the connection
        /// </summary>
        public PacketDecoder? PacketDecoder { get; set; }

        /// <summary>
        /// The data buffer for reading data from the client
        /// </summary>
        public byte[] Buffer { get; private set; }

        /// <summary>
        /// The current state of the connection
        /// </summary>
        public ConnectionState ConnectionState { get; set; }

        /// <summary>
        /// The socket the connection is running through
        /// </summary>
        public Socket ClientSocket { get; private set; }

        /// <summary>
        /// The IP address of the connection
        /// </summary>
        public string IpAddress { get; private set; }

        /// <summary>
        /// The ISAAC encryptor
        /// </summary>
        public ISAACCipher? NetworkEncryptor { get; set; }

        /// <summary>
        /// The ISAAC decryptor
        /// </summary>
        public ISAACCipher? NetworkDecryptor { get; set; }

        /// <summary>
        /// Represents a disconnect
        /// </summary>
        public delegate void Disconnect(Connection connection);

        /// <summary>
        /// Indicates the connection disconnected
        /// </summary>
        public event Disconnect? Disconnected;


        /// <summary>
        /// Creates a new connection
        /// </summary>
        /// <param name="socket">The socket</param>
        public Connection(Socket socket, WorldDetails worldDetails)
        {
            Buffer = new byte[MaxBufferSize];
            ConnectionState = ConnectionState.ConnectionRequest;
            ClientSocket = socket;
            IpAddress = ResolveIpAddress();
        }

        /// <summary>
        /// Resolves the IP address of the client
        /// </summary>
        /// <returns>The IP address</returns>
        private string ResolveIpAddress()
        {
            if (ClientSocket.RemoteEndPoint == null)
            {
                return string.Empty;
            }
            return ((IPEndPoint)ClientSocket.RemoteEndPoint).Address.ToString();
        }

        /// <summary>
        /// Resets the data buffer
        /// </summary>
        /// <returns>The connection</returns>
        public Connection ResetBuffer()
        {
            Buffer = new byte[MaxBufferSize];
            return this;
        }

        /// <summary>
        /// Sends a packet through the socket
        /// </summary>
        /// <param name="writer">The packet</param>
        public Connection Send(PacketWriter writer)
        {
            return Send(writer.Buffer);
        }

        /// <summary>
        /// Sends data through the socket
        /// </summary>
        /// <param name="data">The data</param>
        public Connection Send(byte[] data)
        {
            return Send(data, data.Length);
        }

        /// <summary>
        /// Sends data through the socket
        /// </summary>
        /// <param name="data">The data</param>
        /// <param name="length">The data length</param>
        public Connection Send(byte[] data, int length)
        {
            try
            {
                //ClientSocket.BeginSend(data, 0, length, 0, new AsyncCallback(SendCallback), this);
                ClientSocket.Send(data, 0, length, 0);
            }
            catch (SocketException ex)
            { // Most likely disconnected
                //Console.Error.WriteLine(ex.ToString());
                Debug.WriteLine(ex);
                Dispose();
            }
            catch (ObjectDisposedException ex)
            {
                Debug.WriteLine(ex);
                MarkDisconnected();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Dispose();
            }
            return this;
        }

        /// <summary>
        /// Marks a connection as disconnected
        /// </summary>
        /// <returns>The connection</returns>
        public Connection MarkDisconnected()
        {
            ConnectionState = ConnectionState.Disconnected;
            Disconnected?.Invoke(this);
            return this;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            if (ConnectionState != ConnectionState.Disconnected)
            {
                try
                {
                    ClientSocket.Shutdown(SocketShutdown.Both);
                    ClientSocket.Close();
                    ClientSocket.Dispose();
                }
                catch (ObjectDisposedException) { }
            }
            MarkDisconnected();
        }

    }
}
