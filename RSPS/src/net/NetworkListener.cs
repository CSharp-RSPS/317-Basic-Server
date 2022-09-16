using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net
{
    public class NetworkListener
    {
        public static void AcceptCallback(IAsyncResult result)
        {
            Connection connection = new Connection();
            try
            {

                // Get the socket that handles the client request.  
                Socket socket = (Socket)result.AsyncState;

                connection.clientSocket = socket.EndAccept(result);

                connection.clientSocket.BeginReceive(connection.buffer, 0, connection.buffer.Length, SocketFlags.None,
                    new AsyncCallback(ReadCallback), connection);

                /**
                 * Start accpeting a new connection. Keep the listeners off the main game loop
                 **/
                Program.listener.BeginAccept(new AsyncCallback(AcceptCallback), Program.listener);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Accept callback died");
                System.Environment.Exit(1);
            }
        }

        public static void ReadCallback(IAsyncResult result)
        {
            Connection connection = new Connection();
            try
            {
                // Retrieve the state object and the handler socket  
                // from the asynchronous state object.  
                connection = (Connection)result.AsyncState;

                int bytesRead = connection.clientSocket.EndReceive(result);

                if (bytesRead > 0)
                {
                    MemoryStream tempStream = new MemoryStream(bytesRead);
                    tempStream.Write(connection.buffer, 0, bytesRead);
                    PacketReader packetReader = Packet.CreatePacketReader(tempStream.ToArray());

                    while (packetReader.PayloadPosition < packetReader.Payload.Length)
                    {
                        if (connection.connectionState <= 1)
                        {
                            LoginProtocolDecoder.DecodeLogin(connection);
                            //RSALoginProtocolDecoder.DecodeLogin(connection); - for new client
                            break;
                        }
                        else
                        {
                            int packetOpCode = packetReader.ReadByte() & 0xFF;
                            packetOpCode = (packetOpCode - connection.NetworkDecryptor.getNextValue()) & 0xFF;// -- cryption
                            //Console.WriteLine("packet op code: " + packetOpCode);
                            int packetLength = PACKET_LENGTHS[packetOpCode];
                            if (packetLength == -1)//variable length packet
                            {
                                if (packetReader.PayloadPosition >= packetReader.Payload.Length)
                                {
                                    break;
                                }
                                packetLength = packetReader.ReadByte();
                                packetLength = packetLength & 0xFF;//new
                            }

                            if (packetReader.Payload.Length >= packetLength)
                            {
                                PacketHandler.HandlePacket(connection, packetOpCode, packetLength, packetReader);
                            }

                        }
                    }
                    connection.buffer = new byte[Constants.BufferSize];
                    /**
                     * Start recieving more data.
                     **/
                    connection.clientSocket.BeginReceive(connection.buffer, 0, connection.buffer.Length, SocketFlags.None,
                         new AsyncCallback(ReadCallback), connection);
                }
                
            } catch (Exception ex) { 
                Console.Error.WriteLine(ex.ToString());
                Console.Error.WriteLine("Network listener can't receive packet");
                System.Environment.Exit(1);
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
