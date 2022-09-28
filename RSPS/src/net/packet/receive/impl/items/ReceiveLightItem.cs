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
