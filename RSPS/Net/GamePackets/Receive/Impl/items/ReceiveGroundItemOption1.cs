using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive
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
