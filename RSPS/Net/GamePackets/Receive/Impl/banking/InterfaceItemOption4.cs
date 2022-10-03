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
    /// This packet is sent when a player banks all of a certain item they have in their inventory.
    /// Note: This packet is also used for selling/buying 10 items at a shop.
    /// </summary>
    [PacketInfo(129, 6)]
    public sealed class InterfaceItemOption4 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int slotId = reader.ReadShortAdditional(false);
            int interfaceId = reader.ReadShort(false);
            int itemId = reader.ReadShortAdditional(false);

            switch (interfaceId)
            {
                case Interfaces.InventoryOverlayBank: // Add all to bank
                    break;

                case Interfaces.BankItemsOverlay: // Remove all from bank
                    break;

                case Interfaces.ShopItemsOverlay: // Purchase 10 from shop
                    break;

                case Interfaces.InventoryOverlayShop: // Sell 10 to shop
                    break;

                case Interfaces.InventoryOverlayTrade: // Offer all in trade
                    break;

                case Interfaces.TradeItemsOverlay: // Remove all from trade
                    break;
            }
        }

    }
}
