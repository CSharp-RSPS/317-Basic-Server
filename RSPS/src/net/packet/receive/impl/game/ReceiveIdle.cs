﻿using System;
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
    /// Sent when there are no actions being performed by the player for this cycle.
    /// </summary>
    [Opcode(0)]
    public sealed class ReceiveIdle : IReceivePacket
    {


        public void ReceivePacket(Player player, int packetOpcode, int packetSize, PacketReader packetReader)
        {
            if (player.IdleTimer.IsRunning)
            {
                if (player.IdleTimer.ElapsedMilliseconds > TimeSpan.FromMinutes(5).TotalMilliseconds)
                {
                    Console.WriteLine("Player has been idle for more than 5 minutes");
                }
            } else
            {
                //connection.Player.IdleTimer.Start();
            }
        }
    }
}
