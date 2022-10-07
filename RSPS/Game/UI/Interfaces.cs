using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Banking;
using RSPS.Game.Shopping;
using RSPS.Game.Trading;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send;
using RSPS.Net.GamePackets.Send.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.UI
{
    /// <summary>
    /// Contains interface related operations and utilities
    /// </summary>
    public static class Interfaces
    {

        public const int Inventory = 3214; // The inventory interface items overlay

        public const int Equipment = 1688; // The equipment interface items overlay

        public const int Bank = 5292; // The bank interface
        public const int BankItemsOverlay = 5382; // The bank interface items overlay
        public const int InventoryOverlayBank = 5064; // The inventory overlay while banking

        public const int Shop = 3824; // The shop interface
        public const int ShopItemsOverlay = 3900; // The shop interface items overlay
        public const int InventoryOverlayShop = 3823; // The inventory overlay while shopping
        public const int ShopName = 3901; // The shop name text interface

        public const int Trade = 3323; // The trading interface
        public const int TradeItemsOverlay = 3415; // The trade interface items overlay
        public const int TradePartnerItemsOverlay = 3416; // The trade interface items overlay for the trade partner items
        public const int InventoryOverlayTrade = 3322; // The inventory overlay while trading
        public const int TradeAcceptOverlay = 3443; // The accept screen for a trade
        public const int TradeAcceptItemsOverlay = 3557; // The items overlay on the trade accept screen
        public const int TradeAcceptPartnerItemsOverlay = 3558; // The items overlay for the trade partner on the trade accept screen


        /// <summary>
        /// Retrieves the interface ID of the current inventory overlay
        /// </summary>
        /// <param name="player">The player</param>
        /// <returns>The result</returns>
        public static int GetInventoryRefreshInterface(Player player)
        {
            if (BankingHandler.IsBanking(player))
            {
                return InventoryOverlayBank;
            }
            if (ShopHandler.IsShopping(player))
            {
                return InventoryOverlayShop;
            }
            if (player.NonPersistentVars.IsTrading)
            {
                return player.NonPersistentVars.TradeStage != null && player.NonPersistentVars.TradeStage == TradeStage.AcceptedRequest
                    ? InventoryOverlayTrade : Inventory;
            }
            return Inventory;

        }

        /// <summary>
        /// Attempts to open an interface for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="interfaceId">The interface ID</param>
        public static void OpenInterface(Player player, int interfaceId)
        {
            if (interfaceId < 0) 
            {
                return;
            }
            player.NonPersistentVars.OpenInterfaceId = interfaceId;
            //TODO if banking etc, stop?
            PacketHandler.SendPacket(player, new SendShowInterface(interfaceId));
        }

        /// <summary>
        /// Closes all open interfaces for a player
        /// </summary>
        /// <param name="player">The player</param>
        public static void CloseInterfaces(Player player)
        {
            player.NonPersistentVars.OpenInterfaceId = -1;
            PacketHandler.SendPacket(player, SendPacketDefinition.ClearScreen);
        }

    }
}
