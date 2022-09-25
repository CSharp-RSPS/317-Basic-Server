using RSPS.src.entity.player;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive
{

    /// <summary>
    /// This packet is sent when a player clicks the first option on a ground item.
    /// </summary>
    [PacketInfo(234, 6)]
    public sealed class ReceiveGroundItemOption1 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int itemX = reader.ReadShort(Packet.ByteOrder.LittleEndian);
            int itemY = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
            int itemId = reader.ReadShort(Packet.ValueType.Additional);
        }

    }
}
