using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Items.Equipment;
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
    /// This packet is sent when a player unequips an item.
    /// </summary>
    [PacketInfo(145, 6)]
    public sealed class ReceiveInterfaceItemOption1 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int interfaceId = reader.ReadShortAdditional();
            int itemSlot = reader.ReadShortAdditional();
            int itemId = reader.ReadShortAdditional();

            if (interfaceId < 0 || itemSlot < 0 || itemId < 0)
            {
                return;
            }
            switch (interfaceId)
            {
                case Interfaces.Equipment: // Remove from equipment
                    EquipmentHandler.Unequip(player, itemId, itemSlot);
                    break;

                case Interfaces.InventoryOverlayBank: // Add to bank
                    break;

                case Interfaces.BankItemsOverlay: // Remove from bank
                    break;

                case Interfaces.ShopItemsOverlay: // Shop price check
                    break;

                case Interfaces.InventoryOverlayShop: // Inventory shop price check
                    break;

                case Interfaces.InventoryOverlayTrade: // Offer in trade
                    break;

                case Interfaces.TradeItemsOverlay: // Remove from trade
                    break;
            }
        }

    }
}
