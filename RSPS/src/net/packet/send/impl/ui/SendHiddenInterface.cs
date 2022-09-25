using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sets an interface to be hidden until hovered over.
    /// </summary>
    [PacketDef(PacketDefinition.HiddenInterface)]
    public sealed class SendHiddenInterface : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// Whether to hide until hovered
        /// </summary>
        public bool HideUntilHovered { get; private set; }


        /// <summary>
        /// Creates a new hidden interface payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        /// <param name="hideUntilHovered">Whether to hide until hovered</param>
        public SendHiddenInterface(int interfaceId, bool hideUntilHovered)
        {
            InterfaceId = interfaceId;
            HideUntilHovered = hideUntilHovered;
        }   

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(HideUntilHovered);
            writer.WriteShort(InterfaceId);
        }

    }

}
