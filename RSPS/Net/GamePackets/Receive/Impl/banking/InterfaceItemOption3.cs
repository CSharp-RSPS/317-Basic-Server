using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
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
            int interfaceId = reader.ReadShortLittleEndian();
            int itemId = reader.ReadShortAdditional();
            int slotId = reader.ReadShortAdditional();

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
