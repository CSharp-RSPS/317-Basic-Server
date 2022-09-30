using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Sets the offset for drawing of an interface.
    /// </summary>
    [PacketDef(PacketDefinition.InterfaceOffset)]
    public sealed class SendInterfaceOffset : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// The X offset
        /// </summary>
        public int OffsetX { get; private set; }

        /// <summary>
        /// The Y offset
        /// </summary>
        public int OffsetY { get; private set; }


        /// <summary>
        /// Creates a new interface offset packet payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        /// <param name="offsetX">The X offset</param>
        /// <param name="offsetY">The Y offset</param>
        public SendInterfaceOffset(int interfaceId, int offsetX, int offsetY)
        {
            InterfaceId = interfaceId;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(OffsetX);
            writer.WriteShortLittleEndian(OffsetY);
            writer.WriteShortLittleEndian(InterfaceId);
        }

    }

}
