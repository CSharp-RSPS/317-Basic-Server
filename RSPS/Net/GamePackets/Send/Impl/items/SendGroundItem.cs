using RSPS.Entities.movement.Locations;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Place an item stack on the ground.
    /// </summary>
    [PacketDef(PacketDefinition.SendGroundItem)]
    public sealed class SendGroundItem : IPacketPayloadBuilder
    {

        /// <summary>
        /// The item identifier
        /// </summary>
        public int ItemId { get; private set; }

        /// <summary>
        /// The item quantity
        /// </summary>
        public int Quantity { get; private set; }


        /// <summary>
        /// Creates a new ground item
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <param name="quantity">The item quantity</param>
        public SendGroundItem(int itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortAdditionalLittleEndian(ItemId);
            writer.WriteShort(Quantity);
            writer.WriteByte(0); // ?
        }

    }

}
