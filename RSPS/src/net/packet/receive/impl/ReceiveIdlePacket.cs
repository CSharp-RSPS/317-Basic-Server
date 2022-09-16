using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    public class ReceiveIdlePacket : IReceivePacket
    {
        public void ReceivePacket(Connection connection, PacketReader packetReader)
        {
            if (connection.Player.IdleTimer.IsRunning)
            {
                if (connection.Player.IdleTimer.ElapsedMilliseconds > TimeSpan.FromMinutes(5).TotalMilliseconds)
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
