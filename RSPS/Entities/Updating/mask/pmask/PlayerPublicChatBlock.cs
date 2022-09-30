using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating.block.pblock
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
