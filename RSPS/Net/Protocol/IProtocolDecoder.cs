using RSPS.Net.Connections;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.Codec
{
    /// <summary>
    /// Represents an authentication decoder
    /// </summary>
    public interface IProtocolDecoder
    {


        /// <summary>
        /// Decodes a packet
        /// </summary>
        /// <param name="connection">The connection</param>
        /// <param name="reader">The packet reader</param>
        /// <returns>Whether decoding was successful</returns>
        public bool Decode(Connection connection, PacketReader reader);

    }
}
