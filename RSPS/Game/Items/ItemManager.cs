using RSPS.Data;
using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Items.Consumables;
using RSPS.Game.UI;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using RSPS.Game.Items.Containers;
using System.ComponentModel;
using System.Numerics;

namespace RSPS.Game.Items
{
    /// <summary>
    /// Manages item related operations
    /// </summary>
    public static class ItemManager
    {

        /// <summary>
        /// Holds the item definitions
        /// </summary>
        public static readonly Dictionary<int, ItemDef> Definitions = new();

        /// <summary>
        /// Holds item pricing information
        /// </summary>
        public static readonly Dictionary<int, ItemPrizes> Prizes = new();

        /// <summary>
        /// Holds consumable items
        /// </summary>
        public static readonly Dictionary<int, Consumable> Consumables = new();


        static ItemManager()
        {
            JsonUtil.DataImport<ItemDef>("./Resources/items/item_definitions.json", 
                (elements) => elements.ForEach(def => Definitions.Add(def.Id, def)));

            JsonUtil.DataImport<ItemPrizes>("./Resources/items/item_prizes.json",
               (elements) => elements.ForEach(def => Prizes.Add(def.Id, def)));

            JsonUtil.DataImport<Consumable>("./Resources/items/consumables.json",
               (elements) => elements.ForEach(def => {
                   if (Consumables.ContainsKey(def.Id))
                   {
                       Debug.WriteLine("Duplicate consumable " + def.Id);
                       return;
                   }
                   Consumables.Add(def.Id, def);
               }));
        }

        /// <summary>
        /// Refreshes the interface for an item container
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="container">The item container</param>
        /// <param name="containerInterfaceId">The container interface ID</param>
        public static void RefreshItemContainer(Player player, ItemContainer container, int containerInterfaceId)
        {
            RefreshInterfaceItems(player, container.Items, containerInterfaceId);
        }

        /// <summary>
        /// Refreshes the items on an interface
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="items">The items</param>
        /// <param name="containerInterfaceId">The container interface ID</param>
        public static void RefreshInterfaceItems(Player player, Dictionary<int, Item?> items, int containerInterfaceId)
        {
            PacketHandler.SendPacket(player, new SendDrawItemsOnInterface2(containerInterfaceId, items));
        }

        /// <summary>
        /// Transfers items here from another container for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="itemId">The item identifier</param>
        /// <param name="slot">The item slot</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="sourceContainer">The source container</param>
        /// <param name="targetContainer">The target container</param>
        /// <param name="refreshContainers">The refresh containers action</param>
        public static void TransferContainerItems(Player player, int itemId, int slot, int quantity, 
            ItemContainer sourceContainer, ItemContainer targetContainer, Action refreshContainers)
        {
            Item? inventoryItem = sourceContainer.GetItemBySlot(slot);

            if (inventoryItem == null || inventoryItem.Id != itemId)
            {
                return;
            }
            if (inventoryItem.Amount < quantity)
            {
                quantity = inventoryItem.Amount;
            }
            if (!targetContainer.HasRoomForItems(itemId, quantity))
            {
                PacketHandler.SendPacket(player, new SendMessage("You don't have enough space in your " + targetContainer.Type.ToString().ToLower() +"."));
                return;
            }
            sourceContainer.RemoveItems(itemId, -quantity);
            targetContainer.AddItem(itemId, quantity);
            refreshContainers();
        }

        /// <summary>
        /// Retrieves an item definition for an identifier
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>The item definition</returns>
        public static ItemDef? GetItemDefById(int id)
        {
            return Definitions.ContainsKey(id) ? Definitions[id] : null;
        }

        /// <summary>
        /// Retrieves price information for an item identifier
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>The item prizes</returns>
        public static ItemPrizes? GetItemPrizesById(int id)
        {
            return Prizes.ContainsKey(id) ? Prizes[id] : null;
        }

        /// <summary>
        /// Retrieves whether an item identifier is valid to the game
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <param name="def">The item definition to use if present</param>
        /// <returns>The result</returns>
        public static bool IsValidItemId(int id)
        {
            if (id < 0)
            {
                return false;
            }
            ItemDef? def = GetItemDefById(id);
            return def != null && def.Enabled;
        }

        /// <summary>
        /// Whether an item instance is valid
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>The result</returns>
        public static bool IsValidItem(Item item, out ItemDef? def)
        {
            if (item.Id < 0 || item.Amount <= 0 || string.IsNullOrEmpty(item.Uid))
            {
                def = null;
                return false;
            }
            def = GetItemDefById(item.Id);
            return def != null && def.Enabled && (item.Amount == 1 || def.Stackable);
        }

    }
}
