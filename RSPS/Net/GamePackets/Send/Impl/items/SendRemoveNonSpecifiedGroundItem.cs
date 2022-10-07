using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Remove non-specified ground items
    /// </summary>
    [PacketDef(SendPacketDefinition.RemoveNonSpecifiedGroundItems)]
    public sealed class SendRemoveNonSpecifiedGroundItem : IPacketPayloadBuilder
    {

        /// <summary>
        /// The item identifier
        /// </summary>
        public int ItemId { get; private set; }


        /// <summary>
        /// Removes non-specified ground items
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        public SendRemoveNonSpecifiedGroundItem(int itemId)
        {
            ItemId = itemId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByteAdditional(0); // ? position offset
            writer.WriteShort(ItemId);
        }

    }

}
