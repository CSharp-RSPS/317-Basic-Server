using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send
{
    /// <summary>
    /// Represents a packet payload builder for variable sized packets
    /// </summary>
    public interface IPacketVariablePayloadBuilder : IPacketPayloadBuilder
    {


        /// <summary>
        /// Retrieves the variable payload size
        /// </summary>
        /// <returns>The payload size</returns>
        public int GetPayloadSize();

    }
}
