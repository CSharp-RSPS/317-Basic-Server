using RSPS.src.entity.player;
using RSPS.src.net.packet.receive.impl;
using RSPS.src.net.packet.send;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet
{
    public class PacketHandler
    {
        //stream.createFrame(77); - keeps sending packet 77
        public static void HandlePacket(Connection connection, int packetOpcode, int packetLength, PacketReader packetReader)
        {
            //202 - Empty packet sent on main game loop
            if (packetOpcode > 0) {//fix this later
                connection.Player.IdleTimer.Reset();
            }

            switch (packetOpcode)
            {
                case 0:
                    new ReceiveIdlePacket().ReceivePacket(connection, packetReader);
                    break;

                case 241:
                    new ReceiveClientClick().ReceivePacket(connection, packetReader);
                    break;

                case 248:
                case 164:
                case 98:
                    new ReceiveMovement(packetOpcode, packetLength).ReceivePacket(connection, packetReader);
                    break;

                case 185:
                    new ReceiveButtonClick().ReceivePacket(connection, packetReader);
                    break;

                case 3:
                    new ReceiveFocus().ReceivePacket(connection, packetReader);
                    break;

                case 4:
                    new ReceiveChat(packetLength).ReceivePacket(connection, packetReader);
                    break;

                case 226://Write Background Texture?
                    packetReader.readBytes(packetLength);
                    break;

                case 77://Check for game usages?: lengths 12 or 14
                    packetReader.readBytes(packetLength);
                    break;

                case 86://camera
                    packetReader.readBytes(packetLength);
                    break;

                case 202://client tells us the player is idle! - nothing to read
                    break;

                case 36://validates walking? anti-cheat sends 4 bytes
                    packetReader.ReadInt();
                    break;

                case 121://client finished loading - nothing to read
                    break;

                case 130://interface was closed
                    break;

                default:

                    Console.WriteLine("Unhandled Packet Type: " + packetOpcode + ", length: " + packetLength);
                    packetReader.readBytes(packetLength);
                    break;
            }
        }

        public static void SendPacket(Connection connection, ISendPacket sendPacket)
        {
            try
            {
                // Convert the memoryStream data to buffer
                byte[] byteData = sendPacket.SendPacket(connection.NetworkEncryptor);

                //connection.clientSocket.Send(byteData);
                Program.SendGlobalByes(connection, byteData);
/*                foreach (byte b in byteData)
                {
                    Console.WriteLine(b);
                }*/

                // Begin sending the data to the remote device.  
                //connection.clientSocket.BeginSend(byteData, 0, byteData.Length, 0,
                //    new AsyncCallback(SendCallback), connection.clientSocket);
            } catch (Exception)
            {
                connection.Disconnect(); 
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.

                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);

                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
