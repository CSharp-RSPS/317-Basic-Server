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
    /// Sent when the player walks regularly.
    /// </summary>
    public sealed class ReceiveRegularWalk : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader packetReader)
        {

        }

    }
}