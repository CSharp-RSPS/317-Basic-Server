using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Items;
using RSPS.Game.UI;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Banking
{
    /// <summary>
    /// Handles banking related operations
    /// </summary>
    public static class BankingHandler
    {

        /// <summary>
        /// Holds identifiers of objects in the game world that can be used to bank
        /// </summary>
        public static readonly int[] BankObjects = {
            2213, 2693, 3045, 3194, 4483, 5276, 6084, 10517, 10562, 11338, 11758
            , 65445, 65436, 65476, 65518
        };


        /// <summary>
        /// Retrieves whether a player is banking
        /// </summary>
        /// <param name="player">The player</param>
        /// <returns>The result</returns>
        public static bool IsBanking(Player player)
        {
            return player.NonPersistentVars.OpenInterfaceId == Interfaces.Bank;
        }

        /// <summary>
        /// Opens the bank for a player
        /// </summary>
        /// <param name="player">The player</param>
        public static void OpenBank(Player player)
        {
            // Open the banking interface
            PacketHandler.SendPacket(player, new SendInventoryOverlay(Interfaces.Bank, 5063)); //5063 = hidden bank interface?
            // Draw the inventory items onto the banking inventory overlay
            ItemManager.RefreshInterfaceItems(player, player.Inventory.Items, Interfaces.InventoryOverlayBank);
            // Draw the bank items onto the banking interface
            ItemManager.RefreshInterfaceItems(player, player.Bank.Items, Interfaces.BankItemsOverlay);
            // Update the player's open interface
            player.NonPersistentVars.OpenInterfaceId = Interfaces.Bank;
        }

        /// <summary>
        /// Attempts to deposit items into a player' bank
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <param name="slot">The inventory slot the item is in</param>
        /// <param name="quantity">The item quantity</param>
        public static void Deposit(Player player, int itemId, int slot, int quantity)
        {
            if (!IsBanking(player) || quantity <= 0)
            {
                return;
            }
            Item? inventoryItem = player.Inventory.GetItemBySlot(slot);

            if (inventoryItem == null || inventoryItem.Id != itemId)
            {
                return;
            }
            if (inventoryItem.Amount < quantity)
            {
                quantity = inventoryItem.Amount;
            }
            if (!player.Bank.CanAddItem(inventoryItem))
            {
                PacketHandler.SendPacket(player, new SendMessage("Your bank is full."));
                return;
            }
            player.Inventory.ModifyItemAmount(inventoryItem, slot, -quantity);
            player.Bank.AddItem(new Item(inventoryItem.Id, quantity));
            RefreshBanking(player);
        }

        /// <summary>
        /// Attempts to withdraw items from a player' bank
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="itemId">The item identifier</param>
        /// <param name="slot">The item slot</param>
        /// <param name="quantity">The item quantity</param>
        public static void Withdraw(Player player, int itemId, int slot, int quantity)
        {
            if (!IsBanking(player) || quantity <= 0)
            {
                return;
            }
            Item? itemAtSlot = player.Bank.GetItemBySlot(slot);

            if (itemAtSlot == null || itemAtSlot.Id != itemId)
            {
                return;
            }
            if (itemAtSlot.Amount > quantity)
            {
                quantity = itemAtSlot.Amount;
            }
            Item itemToWithdraw = new(itemAtSlot.Id, quantity);

            if (player.NonPersistentVars.NotedBanking)
            {
                ItemDef? itemDef = ItemManager.GetItemDefById(itemId);

                if (itemDef == null)
                {
                    return;
                }
                if (itemDef.ReverseIdentity != -1)
                {
                    itemToWithdraw = new(itemDef.ReverseIdentity, quantity);
                }
                else
                {
                    PacketHandler.SendPacket(player, new SendMessage("This item can not be withdrawn as a note."));
                }
            }
            if (!player.Inventory.CanAddItem(itemToWithdraw))
            {
                PacketHandler.SendPacket(player, new SendMessage("Your inventory is full."));
                return;
            }
            player.Bank.ModifyItemAmount(itemAtSlot, slot, -quantity);
            player.Inventory.AddItem(itemToWithdraw);
            RefreshBanking(player);
        }

        /// <summary>
        /// Refreshes the banking session for a player
        /// </summary>
        /// <param name="player">The player</param>
        public static void RefreshBanking(Player player)
        {
            ItemManager.RefreshInterfaceItems(player, player.Inventory.Items, Interfaces.BankItemsOverlay);
            ItemManager.RefreshInterfaceItems(player, player.Bank.Items, Interfaces.Bank);
        }

    }
}
