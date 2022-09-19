using RSPS.src.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    public sealed class ReceiveCameraMovement : IReceivePacket
    {


        public void ReceivePacket(Player player, int packetOpCode, int packetLength, PacketReader packetReader)
        {
        }

    }
}
