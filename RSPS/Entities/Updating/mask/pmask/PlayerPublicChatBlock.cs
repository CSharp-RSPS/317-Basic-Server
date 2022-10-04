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
        public void AppendBlock(Player player, PacketWriter writer)
        {
            if (player.Comms.ChatMessage == null)
            {
                return;
            }
            writer.WriteShortLittleEndian(((player.Comms.ChatMessage.Color & 0xff) << 8) + (player.Comms.ChatMessage.Effects & 0xff));
            writer.WriteByte((int)player.PersistentVars.Rights);
            writer.WriteByteNegated(player.Comms.ChatMessage.Text.Length);
            writer.WriteBytesReverse(player.Comms.ChatMessage.Text);

            //player.Comms.ChatMessage = null; // Reset the pending chat message
        }
    }
}
