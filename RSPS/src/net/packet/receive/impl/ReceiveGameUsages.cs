using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.src.entity.player;
using RSPS.src.net.Connections;

namespace RSPS.src.net.packet.receive.impl
{
    public class ReceiveGameUsages : IReceivePacket
    {
        public void ReceivePacket(Player player, int packetOpCode, int packetLength, PacketReader packetReader)
        {
            int value1 = packetReader.ReadByte();
            int value2 = packetReader.ReadByte();
            int value3 = packetReader.ReadByte();

        }
    }
}
