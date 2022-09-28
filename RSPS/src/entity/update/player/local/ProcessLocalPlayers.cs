using RSPS.src.entity.player;
using RSPS.src.entity.update.block;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update.player.local
{
    internal class ProcessLocalPlayers : IUpdateProtocol<Player>
    {
        public void Process(Player player, PacketWriter writer)
        {
            //Update other local players
            writer.WriteBits(8, player.LocalPlayers.Count);

            foreach (Player other in player.LocalPlayers.ToArray())
            {
                if (other.Position.IsWithinDistance(player.Position) && other.PlayerConnection.ConnectionState == ConnectionState.Authenticated && !other.NeedsPlacement)
                {
                    //UpdateOtherPlayerMovement(other, writer);
                    if (other.Flags.IsUpdateNeeded())
                    {
                        new PlayerMask(false, false).Process(player, writer);
                        //UpdateState(other, stateBlock, false, false);
                    }
                }
                else
                {
                    writer.WriteBit(true);
                    writer.WriteBits(2, 3);
                    player.LocalPlayers.Remove(other);
                }

            }
        }
    }
}
