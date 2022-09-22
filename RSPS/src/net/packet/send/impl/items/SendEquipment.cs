﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
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

    public sealed class SendEquipment : ISendPacket
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

        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(13);
            writer.WriteVariableShortHeader(encryptor, 34);
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
            writer.FinishVariableShortHeader();
            return writer;
        }
    }
}