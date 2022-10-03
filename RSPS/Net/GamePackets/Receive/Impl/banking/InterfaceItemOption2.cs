using RSPS.Entities.Mobiles.Players;
using RSPS.Game.UI;
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
    /// This packet is sent when a player attempts to bank 5 of a certain item.
    ///Note: This packet is also used for buying/selling 1 of an item from a shop.
    /// </summary>
    [PacketInfo(117, 6)]
    public sealed class InterfaceItemOption2 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int interfaceId = reader.ReadShortAdditionalLittleEndian();
            int itemId = reader.ReadShortAdditionalLittleEndian();
            int slotId = reader.ReadShortLittleEndian();

            switch (interfaceId)
            {
                case Interfaces.InventoryOverlayBank: // Add 5 to bank
                    break;

                case Interfaces.BankItemsOverlay: // Remove 5 from bank
                    break;

                case Interfaces.ShopItemsOverlay: // Purchase 1 from shop
                    break;

                case Interfaces.InventoryOverlayShop: // Sell 1 to shop
                    break;

                case Interfaces.InventoryOverlayTrade: // Offer 5 in trade
                    break;

                case Interfaces.TradeItemsOverlay: // Remove 5 from trade
                    break;
            }
        }

    }
}
