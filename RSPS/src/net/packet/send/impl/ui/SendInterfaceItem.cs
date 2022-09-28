using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Displays an item model inside an interface.
    /// </summary>
    [PacketDef(PacketDefinition.InterfaceItem)]
    public sealed class SendInterfaceItem : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// The item's model zoom
        /// </summary>
        public int Zoom { get; private set; }

        /// <summary>
        /// The item ID
        /// </summary>
        public int ItemId { get; private set; }


        /// <summary>
        /// Creates a new interface item packet payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        /// <param name="zoom">The item's model zoom</param>
        /// <param name="itemId">The item ID</param>
        public SendInterfaceItem(int interfaceId, int zoom, int itemId)
        {
            InterfaceId = interfaceId;
            Zoom = zoom;
            ItemId = itemId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortLittleEndian(InterfaceId);
            writer.WriteShort(Zoom);
            writer.WriteShort(ItemId);
        }

    }

}
