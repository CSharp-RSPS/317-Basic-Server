using RSPS.src.entity.player;
using RSPS.src.net.Connections;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    public sealed class ReceiveChat : IReceivePacket
    {


        public void ReceivePacket(Player player, int packetOpCode, int packetLength, PacketReader packetReader)
        {
            int effects = packetReader.ReadByte(false, Packet.ValueType.S);
            int color = packetReader.ReadByte(false, Packet.ValueType.S);
            int chatLength = packetLength - 2;
            byte[] text = packetReader.ReadBytesReverse(chatLength, Packet.ValueType.A);

            if (effects < 0 || color < 0 || chatLength < 0 || text == null || text.Length <= 0)
            {
                return;
            }
            player.ChatEffects = effects;
            player.ChatColor = color;
            player.ChatText = text;
            player.ChatUpdateRequired = true;
            player.UpdateRequired = true;
        }
    }
}
