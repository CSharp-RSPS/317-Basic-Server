using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.Connections;

namespace RSPS.src.net.packet.receive
{
    /// <summary>
    /// Represents a packet handler
    /// </summary>
    public interface IReceivePacket
    {

        /// <summary>
        /// Handles a received packet
        /// </summary>
        /// <param name="player">The player that sent the packet</param>
        /// <param name="packetReader">The packet reader</param>
        public void ReceivePacket(Player player, PacketReader reader);

    }
}
