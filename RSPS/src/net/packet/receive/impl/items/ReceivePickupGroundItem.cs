using RSPS.src.entity.player;
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
    /// This packet is sent when a player clicks the "Pick Up" option on an item when its on the ground.
    /// </summary>
    [PacketInfo(236, 6)]
    public sealed class ReceivePickupGroundItem : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int itemY = reader.ReadShort(Packet.ByteOrder.LittleEndian);
            int itemId = reader.ReadShort();
            int itemX = reader.ReadShort(Packet.ByteOrder.LittleEndian);


        }

    }
}
