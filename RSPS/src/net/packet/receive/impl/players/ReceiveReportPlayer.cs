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
    /// This packet is sent when a player reports another player.
    /// </summary>
    [PacketInfo(218, 8)]
    public sealed class ReceiveReportPlayer : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            long playerName = reader.ReadLong(); // Players name as long
            int rule = reader.ReadByte(); // The rule being reported
            int mute = reader.ReadByte(); // Mute for 48 hours - sent as either 1 or 0 for a boolean client-side
        }

    }
}
