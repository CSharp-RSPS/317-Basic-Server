using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Banking;
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

            if (interfaceId < 0 || itemId < 0 || slotId < 0)
            {
                return;
            }
            switch (interfaceId)
            {
                case Interfaces.InventoryOverlayBank: // Add 10 to bank
                    BankingHandler.Deposit(player, itemId, slotId, 10);
                    break;

                case Interfaces.BankItemsOverlay: // Remove 10 from bank
                    BankingHandler.Withdraw(player, itemId, slotId, 10);
                    break;

                case Interfaces.ShopItemsOverlay: // Purchase 5 from shop
                    break;

                case Interfaces.InventoryOverlayShop: // Sell 5 to shop
                    break;

                case Interfaces.InventoryOverlayTrade: // Offer 10 in trade
                    break;

                case Interfaces.TradeItemsOverlay: // Remove 10 from trade
                    break;
            }
        }

    }
}
