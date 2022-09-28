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
    /// This packet is sent when a player unequips an item.
    /// </summary>
    [PacketInfo(145, 6)]
    public sealed class ReceiveInterfaceItemOption1 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int interfaceId = reader.ReadShortAdditional(false);
            int itemSlot = reader.ReadShortAdditional(false);
            int itemId = reader.ReadShortAdditional(false);

            if (interfaceId < 0 || itemSlot < 0 || itemId < 0)
            {
                return;
            }
            switch (interfaceId)
            {
                case 1688: // Remove from equipment
                    break;

                case 5064: // Add to bank
                    break;

                case 5382: // Remove from bank
                    break;

                case 3900: // Shop price check
                    break;

                case 3823: // Inventory shop price check
                    break;

                case 3322: // Offer in trade
                    break;

                case 3415: // Remove from trade
                    break;
            }
        }

    }
}
