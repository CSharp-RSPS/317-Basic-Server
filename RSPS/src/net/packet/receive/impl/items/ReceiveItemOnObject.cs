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
    /// This packet is sent when a player uses an item on object.
    /// Contrary to what most servers have, the packet is 14 and not 12 in size because the client also sends an object rotation.
    /// </summary>
    [PacketInfo(192, 14)]
    public sealed class ReceiveItemOnObject : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int interfaceId = reader.ReadShort(Packet.ValueType.Additional);
            int objectId = reader.ReadShort(Packet.ByteOrder.LittleEndian);
            int objectY = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.BigEndian);
            int itemSlotId = reader.ReadShort(Packet.ByteOrder.BigEndian);
            int objectX = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.BigEndian);
            int itemId = reader.ReadShort();
            int rotation = reader.ReadShort(Packet.ByteOrder.LittleEndian);

            switch (interfaceId)
            {
                case 3214: // Inventory
                    break;
            }
            PacketHandler.SendPacket(player, new SendMessage("Nothing interesting happens."));
        }

    }
}
