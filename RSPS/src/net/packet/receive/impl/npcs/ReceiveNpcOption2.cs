﻿using RSPS.src.entity.player;
using RSPS.src.net.packet.send.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{

    /// <summary>
    /// Sent when a player clicks the second option of an NPC.
    /// </summary>
    public sealed class ReceiveNpcOption2 : IReceivePacket
    {


        public void ReceivePacket(Player player, int packetOpcode, int packetSize, PacketReader packetReader)
        {

        }

    }
}
