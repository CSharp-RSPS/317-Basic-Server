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
    /// This packet is sent when a player banks all of a certain item they have in their inventory.
    /// Note: This packet is also used for selling/buying 10 items at a shop.
    /// </summary>
    [PacketInfo(129, 6)]
    public sealed class InterfaceItemOption4 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int slotId = reader.ReadShort(false, Packet.ValueType.Additional);
            int interfaceId = reader.ReadShort(false);
            int itemId = reader.ReadShort(false, Packet.ValueType.Additional);

            switch (interfaceId)
            {
                case 5064: // Add all to bank
                    break;

                case 5382: // Remove all from bank
                    break;

                case 3900: // Purchase 10 from shop
                    break;

                case 3823: // Sell 10 to shop
                    break;

                case 3322: // Offer all in trade
                    break;

                case 3415: // Remove all from trade
                    break;
            }
        }

    }
}
