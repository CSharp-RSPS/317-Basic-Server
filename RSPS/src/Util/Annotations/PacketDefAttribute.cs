using RSPS.src.net.packet.send;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.Util.Annotations
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
        public PacketDefinition PacketDef { get; private set; }


        /// <summary>
        /// Creates a new packet definition attribute
        /// </summary>
        /// <param name="packetDef">The packet definition attribute</param>
        public PacketDefAttribute(PacketDefinition packetDef)
        {
            PacketDef = packetDef;
        }

    }
}
