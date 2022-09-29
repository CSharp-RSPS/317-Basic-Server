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
    /// This packet is sent when a player clicks the first option of an object, such as "Cut" for trees or "Mine" for rocks.
    /// Most servers use 6 as size and don't include rotation, the packet is actually 8 and includes rotation
    /// </summary>
    [PacketInfo(132, 8)]
    public sealed class ReceiveObjectOption1 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int objectX = reader.ReadShortAdditionalLittleEndian();
            int objectId = reader.ReadShort();
            int objectY = reader.ReadShortAdditional(true);
            int rotation = reader.ReadShortAdditionalLittleEndian();

            Console.WriteLine(objectId + " " + objectX + " " + objectY + " " + rotation);
        }

    }
}
