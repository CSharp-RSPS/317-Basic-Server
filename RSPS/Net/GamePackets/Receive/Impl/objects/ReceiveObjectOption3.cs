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
    /// Sent when the player clicks the third action available for an object.
    /// Most servers use 6 as size and don't include rotation, the packet is actually 8 and includes rotation
    /// However some widespread clients don't have rotation client sided so always check with your client before trying to use rotation
    /// </summary>
    [PacketInfo(70, 8)]
    public sealed class ReceiveObjectOption3 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int objectX = reader.ReadShortLittleEndian();
            int objectY = reader.ReadShort();
            int objectId = reader.ReadShortAdditionalLittleEndian();
            //int rotation = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
        }

    }
}
