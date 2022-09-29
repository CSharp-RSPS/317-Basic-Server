using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.packet.send.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    /// <summary>
    /// This is sent when the player becomes idle and should be logged out. 
    /// This is sent after the player is idle for 60 seconds, after that it is sent every 10 seconds as long as the player is idle.
    /// </summary>
    public sealed class ReceiveIdleLogout : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            PlayerManager.Logout(player);
        }

    }
}
