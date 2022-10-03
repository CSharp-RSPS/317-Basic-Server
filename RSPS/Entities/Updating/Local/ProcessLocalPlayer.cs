using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.Updating.block;
using RSPS.Net.Connections;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating.Local
{
    internal class ProcessLocalPlayer : IUpdateProtocol<Player>
    {
        public void Process(Player player, PacketWriter payload, PacketWriter block)
        {
            payload.WriteBits(8, player.LocalPlayers.Count);

            // ====== Processes all player changes in the region ========
            foreach (Player other in player.LocalPlayers.ToArray())
            {
                if (other.Position.IsWithinDistance(player.Position)
                    && other.PlayerConnection.ConnectionState == ConnectionState.Authenticated
                    && !other.Movement.Teleported)
                {
                    player.Movement.Update(other, payload);

                    if (other.UpdateRequired)
                    {
                        new PlayerMask(false, false).Process(other, block, payload);
                    }
                }
                else
                {
                    payload.WriteBits(1, 1);
                    payload.WriteBits(2, 3);
                    player.LocalPlayers.Remove(other);
                }
            }
        }
    }
}
