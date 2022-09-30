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
    /// Sent when the player should walk somewhere according to a certain action performed, such as clicking an object.
    /// </summary>
    [PacketInfo(98, PacketHeaderType.VariableByte)]
    public sealed class ReceiveWalkOnCommand : ReceiveWalk
    {


        public override void ReceivePacket(Player player, PacketReader packetReader)
        {
            // TODO: Can interact with entity

            HandleWalking(player, packetReader, packetReader.PayloadSize);
        }

    }
}
