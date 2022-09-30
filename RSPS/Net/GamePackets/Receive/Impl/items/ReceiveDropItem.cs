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
    /// This packet is sent when a player wants to drop an item onto the ground.
    /// </summary>
    [PacketInfo(87, 6)]
    public sealed class ReceiveDropItem : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int itemId = reader.ReadShortAdditional();
            int frameId = reader.ReadShort();
            int slotId = reader.ReadShortAdditional();

            if (frameId < 0 || itemId < 0 || slotId < 0)
            {
                return;
            }
            //TODO: drop item
        }

    }
}
