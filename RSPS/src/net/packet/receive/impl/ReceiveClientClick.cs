using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.src.entity.player;
using RSPS.src.net.Connections;

namespace RSPS.src.net.packet.receive.impl
{
    public class ReceiveClientClick : IReceivePacket
    {
        public void ReceivePacket(Player player, int packetOpCode, int packetLength, PacketReader packetReader)
        {
            packetReader.ReadInt(Packet.ByteOrder.BIG);
        }
    }
}
