using RSPS.src.entity.player;
using RSPS.src.net.packet.send.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{

    /// <summary>
    /// Sent when the player should walk somewhere according to a certain action performed, such as clicking an object.
    /// </summary>
    public sealed class ReceiveWalkOnCommand : ReceiveWalk
    {


        public override void ReceivePacket(Player player, PacketReader packetReader)
        {
            // TODO: Can interact with entity

            HandleWalking(player, packetReader, packetReader.PayloadSize);
        }

    }
}
