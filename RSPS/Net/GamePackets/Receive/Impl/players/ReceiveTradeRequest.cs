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
    /// This packet is sent when a player answers a trade request from another player.
    /// </summary>
    [PacketInfo(139, 2)]
    public sealed class ReceiveTradeRequest : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int playerIndex = reader.ReadShortLittleEndian();

            Player? other = WorldHandler.World.Players.ByWorldIndex(playerIndex);

            if (other == null || !other.Position.IsWithinDistance(player.Position))
            {
                return;
            }
        }

    }
}
