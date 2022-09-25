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
    /// This packet is sent when a player clicks the first option of an item, such as "Bury" for bones or "Eat" for food.
    /// </summary>
    [PacketInfo(122, 6)]
    public sealed class ReceiveItemOption1 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int interfaceId = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
            int slot = reader.ReadShort(Packet.ValueType.Additional);
            int itemId = reader.ReadShort(Packet.ByteOrder.LittleEndian);

            switch (interfaceId)
            {
                case 3214: // Inventory
                    break;
            }
        }

    }
}
