using RSPS.src.entity.player;
using RSPS.src.net.packet.send.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{

    /// <summary>
    /// Send when a player clicks the third option of an item.
    /// </summary>
    public sealed class ReceiveItemOption3 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int interfaceId = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
            int slot = reader.ReadShort(Packet.ByteOrder.LittleEndian);
            int itemId = reader.ReadShort(Packet.ValueType.Additional);

            switch (interfaceId)
            {
                case 3214: // Inventory
                    break;
            }
        }

    }
}
