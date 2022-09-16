using RSPS.src.entity;
using RSPS.src.entity.movement;
using RSPS.src.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    public class ReceiveMovement : IReceivePacket
    {

        private int PacketCode;
        private int PacketLength;

        public ReceiveMovement(int packetCode, int packetLength)
        {
            PacketCode = packetCode;
            PacketLength = packetLength;
        }

        public void ReceivePacket(Connection connection, PacketReader packetReader)
        {
            Player player = connection.Player;
            int length = PacketLength;
            if (PacketCode == 248)
            {
                length -= 14;
            }
            int steps = (length - 5) / 2;
            int[,] path = new int[steps, 2];
            int firstStepX = packetReader.ReadShort(Packet.ValueType.A, Packet.ByteOrder.LITTLE);
            for (int i = 0; i < steps; i++)
            {
                path[i, 0] = (sbyte)packetReader.ReadByte();
                path[i, 1] = (sbyte)packetReader.ReadByte();
            }
            int firstStepY = packetReader.ReadShort(Packet.ByteOrder.LITTLE);
            player.MovementHandler.Reset();
            player.MovementHandler.IsRunPath = packetReader.ReadByte(Packet.ValueType.C) == 1;
            player.MovementHandler.AddToPath(new Position(firstStepX, firstStepY));
            for (int i = 0; i < steps; i++)
            {
                path[i, 0] += firstStepX;
                path[i, 1] += firstStepY;
                player.MovementHandler.AddToPath(new Position(path[i, 0], path[i, 1]));
            }
            player.MovementHandler.FinishPath();

            if (PacketCode == 248)
            {
                packetReader.readBytes(14);//client sends additional info we need to get rid of
            }
        }
    }
}
