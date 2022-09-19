using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.src.entity.player;
using RSPS.src.net.Connections;

namespace RSPS.src.net.packet.receive.impl
{
    public class ReceiveIdlePacket : IReceivePacket
    {
        public void ReceivePacket(Player player, int packetOpCode, int packetLength, PacketReader packetReader)
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
