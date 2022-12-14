using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using RSPS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
{

    /// <summary>
    /// This packet is sent when a player clicks the follow option on another player.
    /// </summary>
    [PacketInfo(39, 2)]
    public sealed class ReceiveFollow : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int playerIndex = reader.ReadShortLittleEndian(false);

            if (playerIndex < 0)
            {
                return;
            }
            Player? leader = WorldHandler.World.Players.ByWorldIndex(playerIndex);

            if (leader == null || !leader.Position.IsWithinDistance(player.Position))
            {
                return;
            }
            player.Movement.FollowLeader = leader;
        }

    }
}
