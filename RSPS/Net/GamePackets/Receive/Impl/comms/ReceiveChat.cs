using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.Updating.flag;
using RSPS.Game.Comms.Chatting;
using RSPS.Net.Connections;
using RSPS.Util.Attributes;
using RSPS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
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

            player.ChatMessage = cm;

            //player.Flags.UpdateFlag(FlagType.PublicChat, true);
            player.ChatUpdateRequired = true;
            player.UpdateRequired = true;
        }
    }
}
