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
