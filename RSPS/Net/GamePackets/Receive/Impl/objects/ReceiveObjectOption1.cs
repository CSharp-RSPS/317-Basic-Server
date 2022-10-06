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
    /// This packet is sent when a player clicks the first option of an object, such as "Cut" for trees or "Mine" for rocks.
    /// Most servers use 6 as size and don't include rotation, the packet is actually 8 and includes rotation
    /// </summary>
    [PacketInfo(132, 8)]
    public sealed class ReceiveObjectOption1 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int objectX = reader.ReadShortAdditionalLittleEndian();
            int objectId = reader.ReadShort(false);
            int objectY = reader.ReadShortAdditional();
            int rotation = reader.ReadShortAdditionalLittleEndian();

            Console.WriteLine(objectId + " " + objectX + " " + objectY + " " + rotation);
        }

    }
}
