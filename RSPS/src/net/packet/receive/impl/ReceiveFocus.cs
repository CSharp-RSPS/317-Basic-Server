using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    internal class ReceiveFocus : IReceivePacket
    {
        public void ReceivePacket(Connection connection, PacketReader packetReader)
        {
            bool lostFocus = packetReader.ReadByte() == 0;
            Console.WriteLine("WE lost focus? " + lostFocus);
        }
    }
}
