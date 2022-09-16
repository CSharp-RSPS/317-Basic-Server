using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive
{
    public interface IReceivePacket
    {
        public void ReceivePacket(Connection connection, PacketReader packetReader);
    }
}
