using RSPS.src.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // Receive buffer.  
        public byte[] Buffer = new byte[Constants.BufferSize];

        public Player Player { get; set; }

        public ConnectionState ConnectionState { get; set; }

        public ISAACCipher NetworkEncryptor;
        public ISAACCipher NetworkDecryptor;

        public Socket ClientSocket { get; private set; }

        public bool IsConnected => ClientSocket != null && ClientSocket.Connected;


        public Connection(Socket socket)
        {
            ClientSocket = socket;
            ConnectionState = ConnectionState.Handshake;
        }

        public void SendGlobalByes(byte[] data)
        {
            SendGlobalByes(data, data.Length);
        }

        public void SendGlobalByes(byte[] data, int amount)
        {
            if (!IsConnected)
            {
                Console.WriteLine("Client socket not connected");
                return;
            }
            // Begin sending the data to the remote device.  
            try
            {
                ClientSocket.Send(data, 0, amount, 0);
                //ClientSocket.BeginSend(data, 0, amount, 0, new AsyncCallback(SendCallback), ClientSocket);
            }
            catch (SocketException ex)
            { // Most likely disconnected
                Console.Error.WriteLine(ex.ToString());
                Dispose();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            if (ar.AsyncState == null)
            {
                return;
            }
            try
            {
                // Retrieve the socket from the state object.  
                Socket clientSocket = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = clientSocket.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                Dispose();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            /*
            if (ClientSocket.Connected) {
                ClientSocket.Disconnect(false);
            }
            ClientSocket.Close();*/
            ClientSocket.Dispose();
        }

    }
}
