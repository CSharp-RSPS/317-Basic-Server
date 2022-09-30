using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send
{
    /// <summary>
    /// Represents a packet payload builder
    /// </summary>
    public interface IPacketPayloadBuilder
    {

        /// <summary>
        /// Writer the packet payload
        /// </summary>
        /// <param name="writer">The packet writer</param>
        /// <returns>Whether the payload is valid</returns>
        public void WritePayload(PacketWriter writer);

    }
}
