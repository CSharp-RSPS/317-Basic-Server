using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.src.entity.player;
using RSPS.src.net.Connections;
using RSPS.src.Util.Annotations;

namespace RSPS.src.net.packet.receive.impl
{
    /// <summary>
    /// This packet is sent when the game client window goes in and out of focus.
    /// </summary>
    [PacketInfo(3, 1)]
    public sealed class ReceiveFocusChange : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            bool lostFocus = reader.ReadByte() == 0;
            Console.WriteLine("WE lost focus? " + lostFocus);
        }

    }
}
