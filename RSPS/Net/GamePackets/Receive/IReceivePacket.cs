using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.Entities.Mobiles.Players;
using RSPS.Net.Connections;

namespace RSPS.Net.GamePackets.Receive
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
