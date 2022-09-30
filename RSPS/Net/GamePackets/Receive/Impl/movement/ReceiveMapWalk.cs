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
