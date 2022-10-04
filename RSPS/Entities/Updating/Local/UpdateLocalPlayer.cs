
using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.movement.Locations;
using RSPS.Entities.Updating.block;
using RSPS.Net.Connections;
using RSPS.Net.GamePackets;
using RSPS.Worlds;
using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating.Local
{
    public class UpdateLocalPlayer
    {

        private static readonly int RegionPlayersLimit = 255;

        private static readonly int NewPlayersPerCycle = 45;

        private int addedLocalPlayers = 0;

        public void Process(Player player, PacketWriter payload, PacketWriter block)
        {
            foreach (Player other in WorldHandler.World.Players.Entities.ToArray())
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
                    AddPlayer(player, other, payload, block);
                }
            }
        }

        private void AddPlayer(Player player, Player other, PacketWriter payload, PacketWriter block)
        {
            player.LocalPlayers.Add(other);
            addedLocalPlayers++;
            payload.WriteBits(11, other.WorldIndex); // Server slot.
            payload.WriteBits(1, 1); // Yes, an update is required.
            payload.WriteBits(1, 1); // Discard walking queue(?)
            Position delta = Position.Delta(player.Position, other.Position);
            payload.WriteBits(5, delta.Y);
            payload.WriteBits(5, delta.X);
            new PlayerMask().ForceAppearance().Process(other, block);
        }
    }
}
