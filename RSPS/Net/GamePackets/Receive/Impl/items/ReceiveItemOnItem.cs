using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
{

    /// <summary>
    /// This packet is sent when a player uses an item on another item.
    /// </summary>
    [PacketInfo(53, 4)]
    public sealed class ReceiveItemOnItem : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int itemTargetSlot = reader.ReadShort();
            int itemBeingUsedSlot = reader.ReadShortAdditional();
        }

    }
}
