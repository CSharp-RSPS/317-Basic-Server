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

        public void Process(Player player, PacketWriter writer, PacketWriter stateBlock)
        {
            int addedLocals = 0;
            int worldPlayerCount = WorldHandler.World.Players.Entities.Count;
            for (int i = 0; i < worldPlayerCount; i++)
            {
                if (player.LocalPlayers.Count >= REGION_PLAYERS_LIMIT || addedLocals >= NEW_PLAYERS_PER_CYCLE)
                {
                    break;
                }
                Player other = WorldHandler.World.Players.Entities[i];
                if (other == null || other == player || other.PlayerConnection.ConnectionState != ConnectionState.Authenticated)//so we dont add ourself to the list
                {
                    continue;
                }

                if (player.LocalPlayers.Contains(other) && !other.Position.IsWithinDistance(player.Position))
                {
                    continue;
                }

                player.LocalPlayers.Add(other);
                addedLocals++;
                AddPlayer(writer, player, other);
                new PlayerMask(true, false).Process(other, writer, stateBlock);
                //UpdateState(other, stateBlock, true, false);
            }
        }

        private void AddPlayer(PacketWriter writer, Player player, Player otherPlayer)
        {
            writer.WriteBits(11, otherPlayer.WorldIndex);//Server slot
            writer.WriteBit(true);// Yes an update is required
            writer.WriteBit(true);// Discard walking queue

            Position delta = Position.Delta(player.Position, otherPlayer.Position);
            writer.WriteBits(5, delta.Y);
            writer.WriteBits(5, delta.X);
        }
    }
}
