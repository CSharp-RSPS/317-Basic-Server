﻿using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.packet.send.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{

    /// <summary>
    /// Sent when a players account is flagged.
    /// </summary>
    public sealed class ReceiveFlaggedAccount : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {

        }

    }
}
