using RSPS.src.entity.player;
using RSPS.src.net.Connections;
using RSPS.src.net.packet.receive;
using RSPS.src.net.packet.receive.impl;
using RSPS.src.net.packet.send;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet
{
    public class PacketHandler
    {

        private static readonly Dictionary<int, IReceivePacket> ReceivablePackets = new();


        static PacketHandler() {
            ReceivablePackets.Add(0, new ReceiveIdlePacket());
            ReceivablePackets.Add(3, new ReceiveFocus());
            ReceivablePackets.Add(185, new ReceiveButtonClick());
            ReceivablePackets.Add(241, new ReceiveClientClick());
        }

        //stream.createFrame(77); - keeps sending packet 77
        public static void HandlePacket(Connection connection, int packetOpcode, int packetLength, PacketReader packetReader)
        {
            //202 - Empty packet sent on main game loop
            if (packetOpcode > 0) 
            {//fix this later
                connection.Player.IdleTimer.Reset();
            }
            switch (packetOpcode)
            {
                case 248:
                case 164:
                case 98:
                    new ReceiveMovement(packetOpcode, packetLength).ReceivePacket(connection, packetReader);
                    return;

                case 4:
                    new ReceiveChat(packetLength).ReceivePacket(connection, packetReader);
                    return;

                case 226://Write Background Texture?
                    packetReader.readBytes(packetLength);
                    return;

                case 77://Check for game usages?: lengths 12 or 14
                    packetReader.readBytes(packetLength);
                    return;

                case 86://camera
                    packetReader.readBytes(packetLength);
                    return;

                case 202://client tells us the player is idle! - nothing to read
                    return;

                case 36://validates walking? anti-cheat sends 4 bytes
                    packetReader.ReadInt();
                    return;

                case 121://client finished loading - nothing to read
                    return;

                case 130://interface was closed
                    return;
            }
            if (!ReceivablePackets.ContainsKey(packetOpcode)) {
                packetReader.readBytes(packetLength);
                Console.Error.WriteLine("Unhandled packet {0} (Length: {1})", packetOpcode, packetLength);
                return;
            }
            IReceivePacket packet = ReceivablePackets[packetOpcode];

            packet.ReceivePacket(connection, packetReader);
        }

        public static void SendPacket(Player player, ISendPacket sendPacket)
        {
            if (player.PlayerConnection == null || !player.PlayerConnection.IsConnected)
            {
                Console.Error.WriteLine("Unable to send packet for player {0} because they are not properly connected", player.Credentials.Username);
                return;
            }
            SendPacket(player.PlayerConnection, sendPacket);
        }

        public static void SendPacket(Connection connection, ISendPacket sendPacket)
        {
            try
            {
                // Convert the memoryStream data to buffer
                byte[] byteData = sendPacket.SendPacket(connection.NetworkEncryptor);

                connection.Send(byteData);
/*                foreach (byte b in byteData)
                {
                    Console.WriteLine(b);
                }*/

                // Begin sending the data to the remote device.  
                //connection.clientSocket.BeginSend(byteData, 0, byteData.Length, 0,
                //    new AsyncCallback(SendCallback), connection.clientSocket);
            } catch (Exception)
            {
                connection.Dispose();
            }
        }

    }
}
