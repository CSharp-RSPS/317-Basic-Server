using RSPS.src.entity;
using RSPS.src.entity.movement;
using RSPS.src.entity.player;
using RSPS.src.net.Connections;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    /// <summary>
    /// A base packet for movement related packets we receive
    /// </summary>
    public abstract class ReceiveWalk : IReceivePacket
    {


        public abstract void ReceivePacket(Player player, int packetOpcode, int packetSize, PacketReader packetReader);

        /// <summary>
        /// Handles player walking
        /// </summary>
        /// <param name="player"></param>
        /// <param name="reader"></param>
        /// <param name="walkingDataSize"></param>
        protected static void HandleWalking(Player player, PacketReader reader, int walkingDataSize)
        {
            if (walkingDataSize < 0)
            {
                return;
            }
            //TODO: if can't move - return

            int steps = (walkingDataSize - 5) / 2;

            if (steps < 0)
            {
                return;
            }
            int[,] path = new int[steps, 2];
            int firstStepX = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);

            for (int i = 0; i < steps; i++)
            {
                path[i, 0] = (sbyte)reader.ReadByte();
                path[i, 1] = (sbyte)reader.ReadByte();
            }
            int firstStepY = reader.ReadShort(Packet.ByteOrder.LittleEndian);

            player.MovementHandler.Reset();
            player.MovementHandler.IsRunPath = reader.ReadByte(Packet.ValueType.Negated) == 1;
            player.MovementHandler.AddToPath(new Position(firstStepX, firstStepY));

            for (int i = 0; i < steps; i++)
            {
                path[i, 0] += firstStepX;
                path[i, 1] += firstStepY;

                player.MovementHandler.AddToPath(new Position(path[i, 0], path[i, 1]));
            }
            player.MovementHandler.FinishPath();
        }

    }
}
