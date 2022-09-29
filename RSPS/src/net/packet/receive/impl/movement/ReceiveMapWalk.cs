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
    /// Sent when the player walks using the map. Has 14 additional (assumed to be anticheat) bytes added to the end of it that are ignored.
    /// </summary>
    [PacketInfo(248, PacketHeaderType.VariableByte)]
    public sealed class ReceiveMapWalk : ReceiveWalk
    {


        public override void ReceivePacket(Player player, PacketReader packetReader)
        {
            //TODO: Can map walk

            HandleWalking(player, packetReader, packetReader.PayloadSize - 14);

            packetReader.ReadBytes(14); //client sends additional info we need to get rid of
        }

    }
}
