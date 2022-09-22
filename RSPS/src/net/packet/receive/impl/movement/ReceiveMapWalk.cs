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
    /// Sent when the player walks using the map. Has 14 additional (assumed to be anticheat) bytes added to the end of it that are ignored.
    /// </summary>
    public sealed class ReceiveMapWalk : ReceiveWalk
    {


        public override void ReceivePacket(Player player, int packetOpcode, int packetSize, PacketReader packetReader)
        {
            //TODO: Can map walk

            HandleWalking(player, packetReader, packetSize - 14);

            packetReader.ReadBytes(14); //client sends additional info we need to get rid of
        }

    }
}
