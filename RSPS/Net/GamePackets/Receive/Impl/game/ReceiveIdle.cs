using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.Entities.Mobiles.Players;
using RSPS.Net.Connections;
using RSPS.Util.Attributes;

namespace RSPS.Net.GamePackets.Receive.Impl
{
    /// <summary>
    /// Sent when the player is idle for the current cycle, and acts as a "ping" packet.
    /// </summary>
    [PacketInfo(0, 0)]
    public sealed class ReceiveIdle : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
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
