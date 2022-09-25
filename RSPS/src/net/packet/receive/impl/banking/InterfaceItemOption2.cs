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
    /// This packet is sent when a player attempts to bank 5 of a certain item.
    ///Note: This packet is also used for buying/selling 1 of an item from a shop.
    /// </summary>
    [PacketInfo(117, 6)]
    public sealed class InterfaceItemOption2 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int interfaceId = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
            int itemId = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
            int slotId = reader.ReadShort(Packet.ByteOrder.LittleEndian);

            switch (interfaceId)
            {
                case 5064: // Add 5 to bank
                    break;

                case 5382: // Remove 5 from bank
                    break;

                case 3900: // Purchase 1 from shop
                    break;

                case 3823: // Sell 1 to shop
                    break;

                case 3322: // Offer 5 in trade
                    break;

                case 3415: // Remove 5 from trade
                    break;
            }
        }

    }
}
