using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    public class ReceiveClientClick : IReceivePacket
    {
        public void ReceivePacket(Connection connection, PacketReader packetReader)
        {
            packetReader.ReadInt(Packet.ByteOrder.BIG);
        }
    }
}
