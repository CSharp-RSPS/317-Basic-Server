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
    /// Sent when the player clicks the third action available for an object.
    /// Most servers use 6 as size and don't include rotation, the packet is actually 8 and includes rotation
    /// However some widespread clients don't have rotation client sided so always check with your client before trying to use rotation
    /// </summary>
    [PacketInfo(70, 8)]
    public sealed class ReceiveObjectOption3 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int objectX = reader.ReadShort(Packet.ByteOrder.LittleEndian);
            int objectY = reader.ReadShort();
            int objectId = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
            //int rotation = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
        }

    }
}
