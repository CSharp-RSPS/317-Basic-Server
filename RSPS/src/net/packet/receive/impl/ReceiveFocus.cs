﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.src.entity.player;
using RSPS.src.net.Connections;

namespace RSPS.src.net.packet.receive.impl
{
    internal class ReceiveFocus : IReceivePacket
    {
        public void ReceivePacket(Player player, int packetOpCode, int packetLength, PacketReader packetReader)
        {
            bool lostFocus = packetReader.ReadByte() == 0;
            Console.WriteLine("WE lost focus? " + lostFocus);
        }
    }
}
