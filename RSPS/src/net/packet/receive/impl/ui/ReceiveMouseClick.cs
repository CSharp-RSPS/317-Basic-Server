using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.src.entity.player;
using RSPS.src.net.Connections;

namespace RSPS.src.net.packet.receive.impl
{
    /// <summary>
    /// Sent when the player clicks somewhere on the game screen.
    /// </summary>
    public class ReceiveMouseClick : IReceivePacket
    {

        public void ReceivePacket(Player player, PacketReader packetReader)
        {
            packetReader.ReadInt(Packet.ByteOrder.BigEndian);
        }

    }
}
