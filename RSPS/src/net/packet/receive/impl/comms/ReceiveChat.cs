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
    /// <summary>
    /// Sent when the player enters a chat message.
    /// </summary>
    public sealed class ReceiveChat : IReceivePacket
    {


        public void ReceivePacket(Player player, int packetOpcode, int packetSize, PacketReader packetReader)
        {
            int effects = packetReader.ReadByte(false, Packet.ValueType.Subtrahend);
            int color = packetReader.ReadByte(false, Packet.ValueType.Subtrahend);
            int chatLength = packetSize - 2;
            byte[] text = packetReader.ReadBytesReverse(chatLength, Packet.ValueType.Additional);
            
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
