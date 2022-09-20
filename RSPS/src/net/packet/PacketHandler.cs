using RSPS.src.entity.player;
using RSPS.src.net.Connections;
using RSPS.src.net.packet.receive;
using RSPS.src.net.packet.receive.impl;
using RSPS.src.net.packet.send;
using System.Net.Sockets;

namespace RSPS.src.net.packet
{
    public class PacketHandler
    {

        private static readonly Dictionary<int, IReceivePacket> ReceivablePackets = new();


        static PacketHandler() {
            ReceivablePackets.Add(0, new ReceiveIdlePacket());

            ReceiveMovement movement = new();
            ReceivablePackets.Add(98, movement);
            ReceivablePackets.Add(164, movement);
            ReceivablePackets.Add(248, movement);

            ReceivablePackets.Add(86, new ReceiveCameraMovement());
            ReceivablePackets.Add(41, new ReceiveEquipItem());
            ReceivablePackets.Add(103, new ReceiveCommand());
            ReceivablePackets.Add(185, new ReceiveButtonClick());

            ReceivablePackets.Add(3, new ReceiveFocus());
            
            ReceivablePackets.Add(241, new ReceiveClientClick());
            ReceivablePackets.Add(4, new ReceiveChat());
        }

        //stream.createFrame(77); - keeps sending packet 77
        public static void HandlePacket(Player player, int packetOpcode, int packetLength, PacketReader packetReader)
        {
            //202 - Empty packet sent on main game loop
            
            switch (packetOpcode)
            {

                case 226://Write Background Texture?
                    //packetReader.readBytes(packetLength);
                    break;

                case 77://Check for game usages?: lengths 12 or 14
                    //packetReader.readBytes(packetLength);
                    break;

                case 86://camera
                    //packetReader.readBytes(packetLength);
                    break;

                case 202://client tells us the player is idle! - nothing to read
                    break;

                case 36://validates walking? anti-cheat sends 4 bytes
                    //packetReader.ReadInt();
                    break;

                case 121://client finished loading - nothing to read
                    break;

                case 130://interface was closed
                    break;
            }
            if (!ReceivablePackets.ContainsKey(packetOpcode)) {
                Console.Error.WriteLine("Unhandled packet {0} (Length: {1})", packetOpcode, packetLength);
                packetReader.readBytes(packetLength);
                return;
            }
            if (packetOpcode == 98 || packetOpcode == 164 || packetOpcode == 248)
            {
                Console.WriteLine("movement");
            }
            ReceivablePackets[packetOpcode].ReceivePacket(player, packetOpcode, packetLength, packetReader);
        }

        /// <summary>
        /// Attempts to send a packet to a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="sendPacket">The packet to send</param>
        public static void SendPacket(Player player, ISendPacket sendPacket)
        {
            if (player.PlayerConnection == null)
            {
                Console.Error.WriteLine("Unable to send packet for player {0} because the player has no connection", player.Credentials.Username);
                return;
            }
            SendPacket(player.PlayerConnection, sendPacket);
        }

        /// <summary>
        /// Attempts to send a packet to the client
        /// </summary>
        /// <param name="connection">The client connection</param>
        /// <param name="sendPacket">The packet to send</param>
        public static void SendPacket(Connection connection, ISendPacket sendPacket)
        {
            if (connection.NetworkEncryptor == null)
            {
                Console.Error.WriteLine("Unable to send packet, no encryptor present");
                connection.Dispose();
                return;
            }
            connection.Send(sendPacket.SendPacket(connection.NetworkEncryptor));
        }

    }
}
