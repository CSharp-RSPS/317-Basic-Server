using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// This packet sets the scrollbar position of an interface.
    /// </summary>
    [PacketDef(PacketDefinition.ScrollPosition)]
    public sealed class SendScrollPosition : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// The position of the scrollbar
        /// </summary>
        public int ScrollbarPosition { get; private set; }


        /// <summary>
        /// Creates a new scroll position packet payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        /// <param name="scrollbarPosition">The position of the scrollbar</param>
        public SendScrollPosition(int interfaceId, int scrollbarPosition)
        {
            InterfaceId = interfaceId;
            ScrollbarPosition = scrollbarPosition;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortLittleEndian(InterfaceId)
                .WriteShortAdditional(ScrollbarPosition);
        }

    }

}
