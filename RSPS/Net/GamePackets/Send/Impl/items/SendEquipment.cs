using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{

    //HEAD: 0
    //CAPE: 1
    //AMULET: 2
    //WEAPON: 3
    //CHEST: 4
    //SHIELD: 5
    //LEGS: 7
    //HANDS: 9
    //FEET: 10
    //RING: 12
    //ARROWS: 13

    [PacketDef(SendPacketDefinition.ItemOnInterface)]
    public sealed class SendEquipment : IPacketPayloadBuilder
    {

        private int Slot;
        private int ItemId;
        private int ItemAmount;

        public SendEquipment(int slot, int itemId, int itemAmount)
        {
            Slot = slot;
            ItemId = itemId;
            ItemAmount = itemAmount;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(1688);
            writer.WriteByte(Slot);
            writer.WriteShort((short)ItemId + 1);

            if (ItemAmount > 254)
            {
                writer.WriteByte(255);
                writer.WriteInt(ItemAmount);
            }
            else
            {
                writer.WriteByte(ItemAmount);
            }
        }
    }
}
