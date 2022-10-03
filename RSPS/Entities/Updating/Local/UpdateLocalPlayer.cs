
using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.movement.Locations;
using RSPS.Entities.Updating.block;
using RSPS.Net.Connections;
using RSPS.Net.GamePackets;
using RSPS.Worlds;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating.Local
{
    internal class UpdateLocalPlayer : IUpdateProtocol<Player>
    {

        private static readonly int RegionPlayersLimit = 255;

        private static readonly int NewPlayersPerCycle = 45;

        public void Process(Player player, PacketWriter payload, PacketWriter block)
        {
            int addedLocalPlayers = 0;
            foreach (Player other in WorldHandler.World.Players.Entities)
            {
                if (player.LocalPlayers.Count >= RegionPlayersLimit || addedLocalPlayers >= NewPlayersPerCycle)
                {
                    break;
                }

                if (other == null || other == player || player.LocalPlayers.Contains(other)
                    || other.PlayerConnection.ConnectionState != ConnectionState.Authenticated)
                {
                    continue;
                }

                if (other.Position.IsWithinDistance(player.Position))
                {
                    player.LocalPlayers.Add(other);
                    addedLocalPlayers++;
                    // Add the other player for the player
                    payload.WriteBits(11, other.WorldIndex); // Server slot.
                    payload.WriteBits(1, 1); // Yes, an update is required.
                    payload.WriteBits(1, 1); // Discard walking queue(?)
                    // Write the relative position.
                    Position delta = Position.Delta(player.Position, other.Position);
                    payload.WriteBits(5, delta.Y);
                    payload.WriteBits(5, delta.X);
                    new PlayerMask(true, false).Process(other, block, payload);
                }
            }
        }
    }
}
