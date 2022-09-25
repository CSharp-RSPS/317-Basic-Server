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
    /// This packet is sent when a player uses an item on another item thats on the floor.
    /// </summary>
    [PacketInfo(25, 10)]
    public sealed class ReceiveItemOnFloor : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int interfaceId = reader.ReadShort(Packet.ByteOrder.LittleEndian);
            int itemBeingUsedId = reader.ReadShort(false, Packet.ValueType.Additional);
            int floorItemsId = reader.ReadShort();
            int floorItemY = reader.ReadShort(false, Packet.ValueType.Additional);
            int itemBeingUsedSlot = reader.ReadShort(false, Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
            int floorItemX = reader.ReadShort();
        }

    }
}
