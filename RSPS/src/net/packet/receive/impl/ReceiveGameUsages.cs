using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    public class ReceiveGameUsages : IReceivePacket
    {
        public void ReceivePacket(Connection connection, PacketReader packetReader)
        {
            int value1 = packetReader.ReadByte();
            int value2 = packetReader.ReadByte();
            int value3 = packetReader.ReadByte();

        }
    }
}
