using RSPS.src.entity.movement.Locations;
using RSPS.src.entity.player;
using RSPS.src.entity.update.block;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update.player.local
{
    public class UpdateLocalPlayers : IUpdateProtocol<Player>
    {

        private static readonly int REGION_PLAYERS_LIMIT = 255;
        private static readonly int NEW_PLAYERS_PER_CYCLE = 45;

        public void Process(Player player, PacketWriter writer)
        {
            int addedPlayers = 0;
            World world = WorldHandler.ResolveWorld(player.PlayerConnection);
            int worldPlayers = world == null ? 0 : world.Players.Entities.Count;
            for (int i = 0; i < worldPlayers; i++)
            {
                if (player.LocalPlayers.Count >= REGION_PLAYERS_LIMIT || addedPlayers >= NEW_PLAYERS_PER_CYCLE)
                {
                    break;
                }
                Player other = world.Players.Entities[i];
                if (other == null || other == player || other.PlayerConnection.ConnectionState != ConnectionState.Authenticated)//so we dont add ourself to the list
                {
                    continue;
                }

                if (player.LocalPlayers.Contains(other) && !other.Position.IsWithinDistance(player.Position))
                {
                    continue;
                }

                player.LocalPlayers.Add(other);
                addedPlayers++;
                AddPlayer(writer, player, other);
                new PlayerMask(true, false).Process(other, writer);
                //UpdateState(other, stateBlock, true, false);
            }
        }

        private void AddPlayer(PacketWriter writer, Player player, Player otherPlayer)
        {
            writer.WriteBits(11, otherPlayer.PlayerIndex);//Server slot
            writer.WriteBit(true);// Yes an update is required
            writer.WriteBit(true);// Discard walking queue

            Position delta = Position.Delta(player.Position, otherPlayer.Position);
            writer.WriteBits(5, delta.Y);
            writer.WriteBits(5, delta.X);
        }
    }
}
