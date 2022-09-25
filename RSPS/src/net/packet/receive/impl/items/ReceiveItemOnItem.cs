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
    /// This packet is sent when a player uses an item on another item.
    /// </summary>
    [PacketInfo(53, 4)]
    public sealed class ReceiveItemOnItem : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int itemTargetSlot = reader.ReadShort();
            int itemBeingUsedSlot = reader.ReadShort(Packet.ValueType.Additional);
        }

    }
}
