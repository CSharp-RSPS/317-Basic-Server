using RSPS.Entities;
using RSPS.Entities.movement;
using RSPS.Entities.movement.Locations;
using RSPS.Entities.Mobiles.Players;
using RSPS.Net.Connections;
using RSPS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
{
    /// <summary>
    /// A base packet for movement related packets we receive
    /// </summary>
    public abstract class ReceiveWalk : IReceivePacket
    {


        public abstract void ReceivePacket(Player player, PacketReader packetReader);

        /// <summary>
        /// Handles player walking
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="reader">The packet reader</param>
        /// <param name="walkingDataSize">The walking data size</param>
        protected static void HandleWalking(Player player, PacketReader reader, int walkingDataSize)
        {
            if (walkingDataSize < 0 || !player.Movement.CanMove(player))
            {
                return;
            }
            int steps = (walkingDataSize - 5) / 2;

            if (steps < 0)
            {
                return;
            }
            int[,] path = new int[steps, 2];
            int firstStepX = reader.ReadShortAdditionalLittleEndian();

            for (int i = 0; i < steps; i++)
            {
                path[i, 0] = (sbyte)reader.ReadByte();
                path[i, 1] = (sbyte)reader.ReadByte();
            }
            int firstStepY = reader.ReadShortLittleEndian();

            MovementHandler.PrepareMovement(player);
            player.PlayerMovement.RunningQueueEnabled = reader.ReadByteNegated() == 1;
            MovementHandler.AddExternalStep(player, new Position(firstStepX, firstStepY, player.Position.Z));

            for (int i = 0; i < steps; i++)
            {
                path[i, 0] += firstStepX;
                path[i, 1] += firstStepY;

                MovementHandler.AddExternalStep(player, new Position(path[i, 0], path[i, 1], player.Position.Z));
            }
            player.Movement.FinishMovement();
        }

    }
}
