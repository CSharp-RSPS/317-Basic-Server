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
