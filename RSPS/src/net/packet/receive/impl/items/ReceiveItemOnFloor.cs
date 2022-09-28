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
            int interfaceId = reader.ReadShortLittleEndian();
            int itemBeingUsedId = reader.ReadShortAdditional(false);
            int floorItemsId = reader.ReadShort();
            int floorItemY = reader.ReadShortAdditional(false);
            int itemBeingUsedSlot = reader.ReadShortAdditionalLittleEndian(false);
            int floorItemX = reader.ReadShort();
        }

    }
}
