using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Updates the items in a given interface component.
    /// </summary>
    [PacketDef(PacketDefinition.DrawItemsOnInterface2)]
    public sealed class SendDrawItemsOnInterface2 : IPacketVariablePayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// The amount of items
        /// </summary>
        public int ItemCount { get; private set; }


        /// <summary>
        /// Creates a new draw items on interface packet payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        /// <param name="itemCount">The amount of items</param>
        public SendDrawItemsOnInterface2(int interfaceId, int itemCount)
        {
            InterfaceId = interfaceId;
            ItemCount = itemCount;
        }

        public int GetPayloadSize()
        {
            throw new NotImplementedException();
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(InterfaceId);
            writer.WriteShort(ItemCount);
            //TODO
            for (int i = 0; i < ItemCount; i++)
            {
                int itemQty = 1;
                writer.WriteByte(itemQty);

                if (itemQty == 255)
                {
                    itemQty = 1000;
                    writer.WriteInt(itemQty, Packet.ByteOrder.MiddleEndian);
                }
                int itemId = 0;
                writer.WriteShort(itemId, Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
            }
        }

    }

}
