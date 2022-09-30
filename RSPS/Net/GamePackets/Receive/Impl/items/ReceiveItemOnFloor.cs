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
