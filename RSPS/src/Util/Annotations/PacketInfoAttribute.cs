using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.Util.Annotations
{
    /// <summary>
    /// Represents a packet information attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public sealed class PacketInfoAttribute : Attribute
    {

        /// <summary>
        /// The opcode
        /// </summary>
        public int Opcode { get; private set; }

        /// <summary>
        /// The header type
        /// </summary>
        public PacketHeaderType HeaderType { get; private set; }

        /// <summary>
        /// The payload size
        /// </summary>
        public int PayloadSize { get; private set; }

        /// <summary>
        /// Whether a packet has a payload, this can be either fixed or dynamic (represented by -1)
        /// </summary>
        public bool HasPayload => PayloadSize != 0;

        /// <summary>
        /// Whether the packet has a variable size
        /// </summary>
        public bool HasVariableSize => PayloadSize == -1;


        /// <summary>
        /// Creates a new opcode attribute
        /// </summary>
        /// <param name="opcode">The opcode</param>
        /// <param name="headerType">The header type</param>
        /// <param name="payloadSize">The payload size</param>
        public PacketInfoAttribute(int opcode, PacketHeaderType headerType, int payloadSize = -1)
        {
            Opcode = opcode;
            HeaderType = headerType;
            PayloadSize = payloadSize;

            if (headerType == PacketHeaderType.Fixed && payloadSize == -1)
            {
                throw new ArgumentException("Static header types can not have a payload size of -1");
            }
            if (headerType != PacketHeaderType.Fixed && payloadSize != -1)
            {
                throw new ArgumentException("Variable header types can only have a payload size of -1");
            }
        }

        /// <summary>
        /// Creates a new opcode attribute
        /// </summary>
        /// <param name="opcode">The opcode</param>
        /// <param name="payloadSize">The payload size</param>
        /// <param name="headerType">The header type</param>
        public PacketInfoAttribute(int opcode, int payloadSize, PacketHeaderType headerType = PacketHeaderType.Fixed)
            : this(opcode, headerType, payloadSize) { }

        public override string ToString()
        {
            return string.Format("Opcode: {0}, Header: {1}, Size: {1}", Opcode, Enum.GetName(typeof(PacketHeaderType), HeaderType), PayloadSize);
        }

    }
}
