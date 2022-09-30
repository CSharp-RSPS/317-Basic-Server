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
    /// This packet is sent when a player attempts to light logs on fire.
    /// </summary>
    [PacketInfo(79, 6)]
    public sealed class ReceiveLightItem : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int itemY = reader.ReadShort();
            int itemId = reader.ReadShort(false);
            int itemX = reader.ReadShortLittleEndian();
        }

    }
}
