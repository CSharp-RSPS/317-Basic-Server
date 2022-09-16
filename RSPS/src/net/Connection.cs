using RSPS.src.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net
{
    public class Connection
    {
        // Receive buffer.  
        public byte[] buffer = new byte[Constants.BufferSize];

        // Client socket.
        public Socket clientSocket = null;

        public Player Player;

        public ConnectionState connectionState = ConnectionState.Handshake;
        //0 = connected
        //1 = logging in

        public ISAACCipher NetworkEncryptor;
        public ISAACCipher NetworkDecryptor;

        public void Disconnect()
        {
            Program.RemoveableConnections.Add(Player);
            //if (clientSocket.Connected)
            //{
            //    clientSocket.Disconnect(true);
            //}
        }

    }
}
