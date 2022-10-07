using RSPS.Net.GamePackets.Send;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Util.Attributes
{
    /// <summary>
    /// Represents a packet definition attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PacketDefAttribute : Attribute
    {

        /// <summary>
        /// The packet definition
        /// </summary>
        public SendPacketDefinition PacketDef { get; private set; }


        /// <summary>
        /// Creates a new packet definition attribute
        /// </summary>
        /// <param name="packetDef">The packet definition attribute</param>
        public PacketDefAttribute(SendPacketDefinition packetDef)
        {
            PacketDef = packetDef;
        }

    }
}
