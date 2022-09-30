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
    /// This packet is sent when a player clicks the "Pick Up" option on an item when its on the ground.
    /// </summary>
    [PacketInfo(236, 6)]
    public sealed class ReceivePickupGroundItem : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int itemY = reader.ReadShortLittleEndian();
            int itemId = reader.ReadShort();
            int itemX = reader.ReadShortLittleEndian();


        }

    }
}
