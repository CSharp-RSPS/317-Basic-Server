using RSPS.src.entity.player;
using RSPS.src.entity.update.flag;
using RSPS.src.game.comms.chat;
using RSPS.src.net.Connections;
using RSPS.src.Util.Annotations;
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
    [PacketInfo(3, PacketHeaderType.VariableShort)]
    public sealed class ReceiveChat : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int effects = reader.ReadByteSubtrahend(false);
            int color = reader.ReadByteSubtrahend(false);
            int chatLength = reader.PayloadSize - 2;
            byte[] text = reader.ReadBytesReverse(chatLength, Packet.ValueType.Additional);
            
            if (effects < 0 || color < 0 || chatLength < 0 || text == null || text.Length <= 0)
            {
                return;
            }
            ChatMessage cm = new(effects, color, text);

            player.Flags.UpdateFlag(FlagType.PublicChat, true);

            player.ChatMessage = cm;

            player.ChatUpdateRequired = true;
            player.UpdateRequired = true;
        }
    }
}
