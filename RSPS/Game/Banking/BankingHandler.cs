using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Items;
using RSPS.Game.UI;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send;
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
        /// Closes the bank for a player
        /// </summary>
        /// <param name="player">The player</param>
        public static void CloseBank(Player player)
        {
            Interfaces.CloseInterfaces(player);
            ItemManager.RefreshInterfaceItems(player, player.Inventory.Items, Interfaces.Inventory);
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
            ItemManager.TransferContainerItems(player, itemId, slot, quantity, player.Inventory, player.Bank, () => RefreshBanking(player));
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
            if (!IsBanking(player))
            {
                return;
            }
            ItemManager.TransferContainerItems(player, itemId, slot, quantity, player.Bank, player.Inventory, () => RefreshBanking(player));
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
