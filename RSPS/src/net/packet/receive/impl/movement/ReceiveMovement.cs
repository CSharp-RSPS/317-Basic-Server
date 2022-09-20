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
    public sealed class ReceiveMovement : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader packetReader)
        {
            int length = packetReader.PayloadSize;

            if (length < 0)
            {
                return;
            }
            //TODO: if can't move - return

            if (packetReader.Opcode == 248)
            {
                length -= 14;
            }
            int steps = (length - 5) / 2;

            if (steps < 0)
            {
                return;
            }
            int[,] path = new int[steps, 2];
            int firstStepX = packetReader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);

            for (int i = 0; i < steps; i++)
            {
                path[i, 0] = (sbyte)packetReader.ReadByte();
                path[i, 1] = (sbyte)packetReader.ReadByte();
            }
            int firstStepY = packetReader.ReadShort(Packet.ByteOrder.LittleEndian);

            player.MovementHandler.Reset();
            player.MovementHandler.IsRunPath = packetReader.ReadByte(Packet.ValueType.Negated) == 1;
            player.MovementHandler.AddToPath(new Position(firstStepX, firstStepY));

            for (int i = 0; i < steps; i++)
            {
                path[i, 0] += firstStepX;
                path[i, 1] += firstStepY;

                player.MovementHandler.AddToPath(new Position(path[i, 0], path[i, 1]));
            }
            player.MovementHandler.FinishPath();

            if (packetReader.Opcode == 248)
            {
                packetReader.ReadBytes(14);//client sends additional info we need to get rid of
            }
        }
    }
}
