using RSPS.Game.Items;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Updates the items in a given interface component.
    /// </summary>
    [PacketDef(SendPacketDefinition.DrawItemsOnInterface)]
    public sealed class SendDrawItemsOnInterface : IPacketVariablePayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// The container items
        /// </summary>
        public Dictionary<int, Item?> Items { get; private set; }


        /// <summary>
        /// Creates a new draw items on interface packet payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        /// <param name="items">The container items</param>
        public SendDrawItemsOnInterface(int interfaceId, Dictionary<int, Item?> items)
        {
            InterfaceId = interfaceId;
            Items = items;
        }

        public int GetPayloadSize()
        {
            // 4 initial bytes (2 shorts) + (3-7 * item count) so to be easy we take the max.
            return 4 + Items.Count * 7;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(InterfaceId);
            writer.WriteShort(Items.Count);

            foreach (KeyValuePair<int, Item?> kvp in Items)
            {
                int amount = kvp.Value == null ? 0 : kvp.Value.Amount;
                writer.WriteByte(amount >= 255 ? 255 : amount);

                if (amount >= 255)
                {
                    writer.WriteInt(amount);
                }
                writer.WriteShortAdditionalLittleEndian(kvp.Value == null ? 0 : (kvp.Value.Id + 1));
            }
        }

    }

}
