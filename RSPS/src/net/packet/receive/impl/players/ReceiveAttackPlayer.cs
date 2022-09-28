using RSPS.src.entity.player;
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
    /// Sent when a player selects the attack option on another player.
    /// This packet is sent when a player requests a trade with another player.
    /// </summary>
    [PacketInfo(73, 2)]
    public sealed class ReceiveAttackPlayer : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int playerIndex = reader.ReadShortLittleEndian();

            if (playerIndex < 1)
            {
                return;
            }
            World? world = WorldHandler.ResolveWorld(player);

            if (world == null)
            {
                return;
            }
            Player? target = world.Players.ByWorldIndex(playerIndex);

            if (target == null)
            {
                return;
            }
            // TODO: Engage combat
        }

    }
}
