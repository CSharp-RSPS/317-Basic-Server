using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Draw an item onto an interface.
    /// </summary>
    [PacketDef(SendPacketDefinition.ItemOnInterface)]
    public sealed class SendItemOnInterface : IPacketVariablePayloadBuilder
    {

        /// <summary>
        /// The interface identifier
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// The item identifier
        /// </summary>
        public int ItemId { get; private set; }

        /// <summary>
        /// The item quantity
        /// </summary>
        public int Quantity { get; private set; }


        /// <summary>
        /// Draws an item onto an interface
        /// </summary>
        /// <param name="interfaceId"></param>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        public SendItemOnInterface(int interfaceId, int itemId, int quantity)
        {
            InterfaceId = interfaceId;
            ItemId = itemId;
            Quantity = quantity;
        }

        public int GetPayloadSize()
        {
            return 6 + (Quantity >= 255 ? 2 : 0);
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(InterfaceId);
            writer.WriteByte(0); // ?
            writer.WriteShort(ItemId + 1);
            writer.WriteByte(Quantity >= 255 ? 255 : Quantity);

            if (Quantity >= 255)
            {
                writer.WriteShort(Quantity);
            }
        }

    }

}
