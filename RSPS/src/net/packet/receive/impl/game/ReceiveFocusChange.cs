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
    /// Sent when the game client window goes in and out of focus.
    /// </summary>
    public sealed class ReceiveFocusChange : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader packetReader)
        {
            bool lostFocus = packetReader.ReadByte() == 0;
            Console.WriteLine("WE lost focus? " + lostFocus);
        }

    }
}
