using RSPS.src.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    public class ReceiveChat : IReceivePacket
    {

        private int PacketLength;

        public ReceiveChat(int packetLength)
        {
            PacketLength = packetLength;
        }

        public void ReceivePacket(Connection connection, PacketReader packetReader)
        {
            Player player = connection.Player;
            int effects = packetReader.ReadByte(false, Packet.ValueType.S);
            int color = packetReader.ReadByte(false, Packet.ValueType.S);
            int chatLength = PacketLength - 2;
            byte[] text = packetReader.ReadBytesReverse(chatLength, Packet.ValueType.A);
            player.ChatEffects = effects;
            player.ChatColor = color;
            player.ChatText = text;
            player.ChatUpdateRequired = true;
            player.UpdateRequired = true;
        }
    }
}
