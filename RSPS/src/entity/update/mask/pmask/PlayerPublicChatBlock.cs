using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update.block.pblock
{
    public class PlayerPublicChatBlock : IUpdateMask<Player>
    {
        public void ProcessBlock(Player player, PacketWriter writer)
        {
            writer.WriteShortLittleEndian(((player.ChatMessage.Color & 0xff) << 8) + (player.ChatMessage.Effects & 0xff));
            writer.WriteByte((int)player.Rights);
            writer.WriteByteNegated(player.ChatMessage.Text.Length);
            writer.WriteBytesReverse(player.ChatMessage.Text);
        }
    }
}
