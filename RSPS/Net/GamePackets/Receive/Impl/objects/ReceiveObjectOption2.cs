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
    /// This packet is sent when a player clicks the second option available of an object, such as "Prospect" for rocks.
    /// Most servers use 6 as size and don't include rotation, the packet is actually 8 and includes rotation
    /// However some widespread clients don't have rotation client sided so always check with your client before trying to use rotation
    /// </summary>
    [PacketInfo(252, 8)]
    public sealed class ReceiveObjectOption2 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int objectId = reader.ReadShortAdditionalLittleEndian();
            int objectX = reader.ReadShortLittleEndian();
            int objectY = reader.ReadShortAdditional();
            // int rotation = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
        }

    }
}
