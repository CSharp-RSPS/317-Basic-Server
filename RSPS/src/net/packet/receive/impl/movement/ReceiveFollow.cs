using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util.Annotations;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
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
            Player? leader = WorldHandler.World.Players.ByPlayerIndex(playerIndex);

            if (leader == null || !leader.Position.IsWithinDistance(player.Position))
            {
                return;
            }
            //TODO follow
        }

    }
}
