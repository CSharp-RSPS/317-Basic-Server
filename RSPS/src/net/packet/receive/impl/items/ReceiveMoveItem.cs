using RSPS.src.entity.player;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{

    /// <summary>
    /// This packet is sent when a player moves an item from one slot to another.
    /// </summary>
    [PacketInfo(214, 7)]
    public sealed class ReceiveMoveItem : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int interfaceId = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
            int insertMode = reader.ReadByte();
            int startingSlot = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
            int newSlot = reader.ReadShort(Packet.ByteOrder.LittleEndian);

            switch (interfaceId)
            {
                case 3214: //Inventory
                case 5064: //Inventory
                    break;

                case 5382: // Bank
                    break;
            }
        }

    }
}
