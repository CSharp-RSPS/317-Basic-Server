using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{

    /// <summary>
    /// This packet is sent when a player changes their privacy options (i.e. public chat).
    /// </summary>
    [PacketInfo(95, 3)]
    public sealed class ReceivePrivacyOptions : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int publicChatOptions = reader.ReadByte(false);
            int privateChatOptions = reader.ReadByte(false);
            int tradeOptions = reader.ReadByte(false);
        }

    }
}
