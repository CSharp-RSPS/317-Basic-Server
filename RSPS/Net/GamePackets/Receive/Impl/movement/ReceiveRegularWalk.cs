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
    /// Sent when the player walks regularly.
    /// </summary>
    [PacketInfo(164, 8)]
    public sealed class ReceiveRegularWalk : ReceiveWalk
    {


        public override void ReceivePacket(Player player, PacketReader packetReader)
        {
            HandleWalking(player, packetReader, packetReader.PayloadSize);
        }

    }
}
