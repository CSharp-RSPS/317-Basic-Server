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
            Player? target = WorldHandler.World.Players.ByWorldIndex(playerIndex);

            if (target == null)
            {
                return;
            }
            // TODO: Engage combat
        }

    }
}
