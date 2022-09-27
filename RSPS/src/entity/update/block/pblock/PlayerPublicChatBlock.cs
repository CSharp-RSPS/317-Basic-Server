using RSPS.src.entity.player;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update.block.pblock
{
    public class PlayerPublicChatBlock : IUpdateBlock<Player>
    {
        public void ProcessBlock(Player player, PacketWriter writer)
        {
            writer.WriteShort(((player.ChatMessage.Color & 0xff) << 8) + (player.ChatMessage.Effects & 0xff), Packet.ByteOrder.LittleEndian);
            writer.WriteByte((int)player.Rights);
            writer.WriteByte(player.ChatMessage.Text.Length, Packet.ValueType.Negated);
            writer.WriteBytesReverse(player.ChatMessage.Text);
        }
    }
}
