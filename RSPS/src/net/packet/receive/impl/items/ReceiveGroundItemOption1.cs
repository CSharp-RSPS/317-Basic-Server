using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive
{

    /// <summary>
    /// This packet is sent when a player clicks the first option on a ground item.
    /// </summary>
    [PacketInfo(234, 6)]
    public sealed class ReceiveGroundItemOption1 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int itemX = reader.ReadShortLittleEndian();
            int itemY = reader.ReadShortAdditionalLittleEndian();
            int itemId = reader.ReadShortAdditional();
        }

    }
}
