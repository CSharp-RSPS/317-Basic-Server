using RSPS.Entities.Mobiles.Players;
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

        public const int Inventory = 3214;
        public const int InventoryOverlayBank = 5064;
        public const int InventoryOverlayShop = 3823;
        public const int InventoryOverlayTrade = 3322;

        public const int Bank = 5292;
        public const int BankItemsOverlay = 5382;
        public const int ShopItemsOverlay = 3900;
        public const int TradeItemsOverlay = 3415;
        public const int Equipment = 1688;


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
            PacketHandler.SendPacket(player, PacketDefinition.ClearScreen);
        }

    }
}
