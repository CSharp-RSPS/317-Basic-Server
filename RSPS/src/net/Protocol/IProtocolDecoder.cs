using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.Codec
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
        /// <param name="packet">The packet</param>
        /// <returns>Whether decoding was successful</returns>
        public bool Decode(Connection connection, PacketReader packet);

    }
}
