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
    /// This packet is sent when the player attempts to bank 10 of a certain item.
    /// Note: This packet is also used for selling/buying 5 of an item from a shop.
    /// </summary>
    [PacketInfo(43, 6)]
    public sealed class InterfaceItemOption3 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int interfaceId = reader.ReadShort(Packet.ByteOrder.LittleEndian);
            int itemId = reader.ReadShort(Packet.ValueType.Additional);
            int slotId = reader.ReadShort(Packet.ValueType.Additional);

            switch (interfaceId)
            {
                case 5064: // Add 10 to bank
                    break;

                case 5382: // Remove 10 from bank
                    break;

                case 3900: // Purchase 5 from shop
                    break;

                case 3823: // Sell 5 to shop
                    break;

                case 3322: // Offer 10 in trade
                    break;

                case 3415: // Remove 10 from trade
                    break;
            }
        }

    }
}
