using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Banking;
using RSPS.Game.Items;
using RSPS.Game.Items.Containers;
using RSPS.Game.Shopping;
using RSPS.Game.UI;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send;
using RSPS.Net.GamePackets.Send.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Trading
{
    /// <summary>
    /// Handles trading related operations
    /// </summary>
    public static class TradingHandler
    {


        /// <summary>
        /// Validates a (potential) trade
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="other">The (potential) trade partner</param>
        /// <param name="stage">The expected trade stage</param>
        /// <returns>The result</returns>
        private static bool ValidateTrade(Player player, Player other, TradeStage? stage = null)
        {
            if (!player.LoggedIn)
            {
                return false;
            }
            if (BankingHandler.IsBanking(other) || ShopHandler.IsShopping(other) 
                || other.IsDisabled || !other.LoggedIn)
            {
                PacketHandler.SendPacket(player, new SendMessage("This player is currently busy."));
                return false;
            }
            if (!player.Position.IsWithinDistance(other.Position))
            {
                PacketHandler.SendPacket(player, new SendMessage("You are too far away of this player."));
                return false;
            }
            if (stage != null && player.NonPersistentVars.TradeStage != stage)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Requests a trade with a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="other">The other player</param>
        public static void RequestTrade(Player player, Player other)
        { //TODO: cooldowns
            if (other.NonPersistentVars.IsTrading)
            {
                PacketHandler.SendPacket(player, new SendMessage("This player is already in a trade."));
                return;
            }
            if (!ValidateTrade(player, other))
            {
                return;
            }
            PacketHandler.SendPacket(other, new SendMessage(other.Credentials.Username + ":tradereq:"));
            PacketHandler.SendPacket(player, new SendMessage("You sent " + other.Credentials.Username + " a trade request."));
        }

        /// <summary>
        /// Accepts a trade with a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="other">The player that requested the trade</param>
        public static void AcceptTradeRequest(Player player, Player other)
        {
            StartTrade(player, other);
            StartTrade(other, player);
        }

        /// <summary>
        /// Starts trading with a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="other">The trade partner</param>
        public static void StartTrade(Player player, Player other)
        {
            if (!ValidateTrade(player, other))
            {
                return;
            }
            player.NonPersistentVars.TradeContainer = new ItemContainer(ItemContainerType.Trade, player.PersistentVars.Member);
            player.NonPersistentVars.TradePartner = other;
            player.NonPersistentVars.TradeStage = TradeStage.AcceptedRequest;

            // Open the trade interface
            PacketHandler.SendPacket(player, new SendInventoryOverlay(Interfaces.Trade, 3321)); //3321 = hidden trade interface?
            // Draw the inventory items onto the trading inventory overlay
            ItemManager.RefreshInterfaceItems(player, player.Inventory.Items, Interfaces.InventoryOverlayTrade);
            // Draw the trade items onto the trading interface
            ItemManager.RefreshInterfaceItems(player, player.NonPersistentVars.TradeContainer.Items, Interfaces.TradeItemsOverlay);
            ItemManager.RefreshInterfaceItems(player, new(), Interfaces.TradePartnerItemsOverlay);

            PacketHandler.SendPacket(player, new SendSetInterfaceText(3431, "3431 test")); // not sure what this overwrites
            PacketHandler.SendPacket(player, new SendSetInterfaceText(3535, "Are you sure you want to make this trade?"));
            PacketHandler.SendPacket(player, new SendSetInterfaceText(3417, "Trading with: @whi@" + other.Credentials.Username +
                    " who has @gre@" + other.Inventory.GetFreeSlots() + " free slots."));
        }

        public static void Offer(Player player, int itemId, int slot, int quantity)
        {
            if (player.NonPersistentVars.TradePartner == null 
                || !ValidateTrade(player, player.NonPersistentVars.TradePartner, TradeStage.AcceptedRequest))
            {
                StopTrade(player, true);
                return;
            }

        }

        public static void WithdrawOffer(Player player, int itemId, int slot, int quantity)
        {
            if (player.NonPersistentVars.TradePartner == null
                || !ValidateTrade(player, player.NonPersistentVars.TradePartner, TradeStage.AcceptedRequest))
            {
                StopTrade(player, true);
                return;
            }
        }

        /// <summary>
        /// Confirms an ongoing trade for a player
        /// </summary>
        /// <param name="player">The player</param>
        public static void ConfirmTrade(Player player)
        {
            if (player.NonPersistentVars.TradeStage == TradeStage.Confirmed)
            {
                return;
            }
            if (player.NonPersistentVars.TradePartner == null
                || !ValidateTrade(player, player.NonPersistentVars.TradePartner, TradeStage.AcceptedRequest))
            {
                StopTrade(player, true);
                return;
            }
            if (player.NonPersistentVars.TradePartner.NonPersistentVars.TradeContainer == null)
            {
                StopTrade(player, true);
                return;
            }
            if (player.NonPersistentVars.TradePartner.NonPersistentVars.TradeContainer.GetFreeSlots() 
                < player.NonPersistentVars.TradeContainer?.GetFilledSlots())
            {
                PacketHandler.SendPacket(player, new SendMessage("Your trade partner doesn't have enough space in their inventory."));
                return;
            }
            player.NonPersistentVars.TradeStage = TradeStage.Confirmed;

            if (player.NonPersistentVars.TradePartner.NonPersistentVars.TradeStage == TradeStage.Confirmed)
            {
                OpenAcceptTradeScreen(player);
                OpenAcceptTradeScreen(player.NonPersistentVars.TradePartner);
                return;
            }
            PacketHandler.SendPacket(player, new SendSetInterfaceText(3431, "Waiting for other player..."));
            PacketHandler.SendPacket(player.NonPersistentVars.TradePartner, new SendSetInterfaceText(3431, "Other player has accepted"));
        }

        /// <summary>
        /// Opens the accept trade screen for a player
        /// </summary>
        /// <param name="player">The player</param>
        private static void OpenAcceptTradeScreen(Player player)
        {
            if (player.NonPersistentVars.TradePartner == null 
                || player.NonPersistentVars.TradeStage != TradeStage.Confirmed)
            {
                StopTrade(player, true);
                return;
            }
            ItemManager.RefreshInterfaceItems(player, player.Inventory.Items, Interfaces.Inventory);
            PacketHandler.SendPacket(player, new SendInventoryOverlay(Interfaces.TradeAcceptOverlay, 3213)); // 3213?
            PacketHandler.SendPacket(player, new SendSetInterfaceText(Interfaces.TradeAcceptItemsOverlay, TradeOfferAsText(player)));
            PacketHandler.SendPacket(player, new SendSetInterfaceText(Interfaces.TradeAcceptPartnerItemsOverlay, 
                TradeOfferAsText(player.NonPersistentVars.TradePartner)));
        }

        /// <summary>
        /// Accepts a trade for a player
        /// </summary>
        /// <param name="player">The player</param>
        public static void AcceptTrade(Player player)
        {
            if (player.NonPersistentVars.TradeStage == TradeStage.Accepted)
            {
                return;
            }
            if (player.NonPersistentVars.TradePartner == null
                || !ValidateTrade(player, player.NonPersistentVars.TradePartner, TradeStage.Confirmed))
            {
                StopTrade(player, true);
                return;
            }
            player.NonPersistentVars.TradeStage = TradeStage.Accepted;

            if (player.NonPersistentVars.TradePartner.NonPersistentVars.TradeStage == TradeStage.Accepted)
            { // Other player has already accepted
                //TODO add items
                if (player.NonPersistentVars.TradePartner.NonPersistentVars.TradeContainer != null)
                {
                    TransferItems(player, player.NonPersistentVars.TradePartner.NonPersistentVars.TradeContainer);
                    StopTrade(player, false);
                }
                if (player.NonPersistentVars.TradeContainer != null)
                {
                    TransferItems(player.NonPersistentVars.TradePartner, player.NonPersistentVars.TradeContainer);
                    StopTrade(player.NonPersistentVars.TradePartner, false);
                }
                return;
            }
            PacketHandler.SendPacket(player, new SendSetInterfaceText(3535, "Waiting for other player..."));
            PacketHandler.SendPacket(player.NonPersistentVars.TradePartner, new SendSetInterfaceText(3535, "Other player has accepted"));
        }

        /// <summary>
        /// Transfers the items from a trade container to a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="tradeContainer">The trade container</param>
        private static void TransferItems(Player player, ItemContainer tradeContainer)
        {
            foreach (Item? item in tradeContainer.Items.Values)
            {
                if (item != null)
                {
                    int quantityTransfered = player.Inventory.AddItem(item, true);

                    if (quantityTransfered < item.Amount)
                    {
                        throw new Exception("This shouldn't happen, but just incase we don't want this to happen");
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves a trade offer for a player as text
        /// </summary>
        /// <param name="player">The player</param>
        /// <returns>The text</returns>
        private static string TradeOfferAsText(Player player)
        {
            if (player.NonPersistentVars.TradeContainer == null)
            {
                return string.Empty;
            }
            string tradeItems = "Absolutely nothing!";
            string tradeAmount = "";

            int count = 0;

            foreach (Item? item in player.NonPersistentVars.TradeContainer.Items.Values)
            {
                ItemDef? itemDef = item == null ? null : ItemManager.GetItemDefById(item.Id);

                if (item == null || itemDef == null)
                {
                    continue;
                }
                if (item.Amount >= 1000 && item.Amount < 1000000)
                {
                    tradeAmount = "@cya@" + (item.Amount / 1000) + "K @whi@(" + item.Amount + ")";
                }
                else if (item.Amount >= 1000000)
                {
                    tradeAmount = "@gre@" + (item.Amount / 1000000) + " million @whi@(" + item.Amount + ")";
                }
                else
                {
                    tradeAmount = "" + item.Amount;
                }

                if (count == 0)
                {
                    tradeItems = itemDef.Name;
                }
                else
                {
                    tradeItems = tradeItems + "\\n" + itemDef.Name;
                }

                if (itemDef.Stackable)
                {
                    tradeItems = tradeItems + " x " + tradeAmount;
                }
                count++;
            }
            return tradeItems;
        }

        /// <summary>
        /// Stops a trade for a player
        /// </summary>
        /// <param name="player">The player</param>
        public static void StopTrade(Player player, bool interruped)
        {
            if (player.NonPersistentVars.TradeContainer != null)
            { // Put any items that are in the container back into the inventory
                foreach (Item? item in player.NonPersistentVars.TradeContainer.Items.Values)
                {
                    if (item != null)
                    {
                        player.Inventory.AddItem(item);
                    }
                }
            }
            player.NonPersistentVars.TradeContainer = null;

            if (player.NonPersistentVars.TradePartner != null)
            {
                if (interruped)
                {
                    PacketHandler.SendPacket(player.NonPersistentVars.TradePartner, new SendMessage(player.Credentials.Username + " has declined the trade."));
                }
                StopTrade(player.NonPersistentVars.TradePartner, false);
                player.NonPersistentVars.TradePartner = null;
            }
            player.NonPersistentVars.TradeStage = null;
            Interfaces.CloseInterfaces(player);
            ItemManager.RefreshInterfaceItems(player, player.Inventory.Items, Interfaces.Inventory);
        }

    }
}
