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
