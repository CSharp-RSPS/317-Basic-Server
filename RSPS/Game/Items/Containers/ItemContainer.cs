using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items.Containers
{
    /// <summary>
    /// Represents a container for items
    /// </summary>
    public class ItemContainer
    {

        /// <summary>
        /// The item container type
        /// </summary>
        public ItemContainerType Type { get; private set; }

        /// <summary>
        /// The capacity of the container
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// Whether the container is for a member
        /// </summary>
        public bool Member { get; private set; }

        /// <summary>
        /// Holds the items in the container
        /// </summary>
        public Dictionary<int, Item?> Items { get; private set; }

        /// <summary>
        /// Whether to always stack items in the container
        /// </summary>
        private readonly bool alwaysStack;


        /// <summary>
        /// Creates a new item container
        /// </summary>
        /// <param name="type">The item container type</param>
        /// <param name="member">Whether the container is used by a member</param>
        public ItemContainer(ItemContainerType type, bool member)
        {
            Type = type;
            Member = member;

            ItemContainerTypeAttribute attr = EnumUtil.GetAttributeOfType<ItemContainerTypeAttribute>(type);
            int maxCapacity = attr.MemberCapacity;
            Capacity = attr.Capacity;
            alwaysStack = attr.AlwaysStack;

            Items = new Dictionary<int, Item?>(maxCapacity);

            for (int i = 0; i < maxCapacity; i++)
            { // Initialize the container
                Items.Add(i, null);
            }
        }

        /// <summary>
        /// Retrieves the amount slots that are free to use
        /// </summary>
        /// <returns>The count</returns>
        public int GetFreeSlots()
        {
            int filledSlots = GetFilledSlots();
            return filledSlots >= Capacity ? 0 : Capacity - filledSlots;
        }

        /// <summary>
        /// Retrieves the amount of slots in use
        /// </summary>
        /// <returns>The count</returns>
        public int GetFilledSlots()
        {
            return Items.Where(kvp => kvp.Value != null).ToList().Count;
        }

        /// <summary>
        /// Retrieves whether an item is present in the container
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <param name="quantity">The quantity of the item</param>
        /// <returns>The result</returns>
        public bool HasItem(int itemId, int quantity = 1)
        {
            return GetAmountOfItem(itemId) >= quantity;
        }

        /// <summary>
        /// Retrieves the amount of an item present in the container
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <returns>The item amount</returns>
        public int GetAmountOfItem(int itemId)
        {
            return Items.Values.Sum(item => item == null || item.Id != itemId ? 0 : item.Amount);
        }

        /// <summary>
        /// Retrieves the items in the container
        /// </summary>
        /// <returns>The items</returns>
        public List<Item> GetItems()
        {
            List<Item> items = new();
            
            foreach (Item? item in Items.Values)
            {
                if (item != null)
                {
                    items.Add(item);
                }
            }
            return items;
        }

        /// <summary>
        /// Retrieves an item by it's unique identifier
        /// </summary>
        /// <param name="uid">The unique identifier</param>
        /// <returns>The item</returns>
        public Item? GetItemByUid(string uid)
        {
            return Items.Values.FirstOrDefault(item => item != null && item.Uid == uid);
        }

        /// <summary>
        /// Retrieves the items in the container for a given item identifier
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <returns>The items</returns>
        public List<Item> GetItemsById(int itemId)
        {
            return GetItems().Where(item => item.Id == itemId).ToList();
        }

        /// <summary>
        /// Retrieves the first item found with a given item identifier
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <returns>The item</returns>
        public Item? GetItemById(int itemId)
        {
            return GetItems().FirstOrDefault(item => item.Id == itemId);
        }

        /// <summary>
        /// Retrieves the slot for the first item found with a given item identifier
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <returns>The slot</returns>
        public int? GetItemSlot(int itemId)
        {
            Item? item = GetItemById(itemId);
            return item == null ? null : GetItemSlot(item);
        }

        /// <summary>
        /// Retrieves the slot for a given item
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>The slot</returns>
        public int? GetItemSlot(Item item)
        {
            return GetItemSlot(item.Id);
        }

        /// <summary>
        /// Retrieves an item by it's slot
        /// </summary>
        /// <param name="slot">The slot</param>
        /// <returns>The item</returns>
        public Item? GetItemBySlot(int slot)
        {
            return Items[slot];
        }

        /// <summary>
        /// Retrieves whether an item with a given identitfier is present in a given slot
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <param name="slot">The slot</param>
        /// <returns>The result</returns>
        public bool HasItemInSlot(int itemId, int slot)
        {
            Item? itemInSlot = GetItemBySlot(slot);
            return itemInSlot != null && itemInSlot.Id == itemId;
        }

        /// <summary>
        /// Retrieves whether an item is present in a given slot
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="slot">The slot</param>
        /// <returns>The result</returns>
        public bool HasItemInSlot(Item item, int slot)
        {
            Item? itemInSlot = GetItemBySlot(slot);
            return itemInSlot != null && itemInSlot.Uid == item.Uid;
        }

        /// <summary>
        /// Retrieves the first available slot for a given item identifier
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <returns>The result</returns>
        public int? GetFirstAvailableSlot(int itemId)
        {
            ItemDef? itemDef = ItemManager.GetItemDefById(itemId);

            if (itemDef == null)
            {
                return null;
            }
            Item? sameIdItem = GetItemById(itemId);

            if (sameIdItem != null && (itemDef.Stackable || alwaysStack) && sameIdItem.Amount < int.MaxValue)
            { // If there's an item in the container with the same ID and the item is stackable we can use that slot unless it's amount is maxed
                return GetItemSlot(sameIdItem);
            }
            if (GetFreeSlots() <= 0)
            { // There are no free slots available
                return null;
            }
            // Find the first free slot
            return Items.FirstOrDefault(kvp => kvp.Value == null).Key;
        }

        /// <summary>
        /// Retrieves whether there is room in the container for a given item
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>The result</returns>
        public bool HasRoomForItem(Item item)
        {
            return HasRoomForItems(item.Id, item.Amount);
        }

        /// <summary>
        /// Retrieves whether there is room in the container for a given quantity of an item
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <param name="quantity">The quantity</param>
        /// <returns>The result</returns>
        public bool HasRoomForItems(int itemId, int quantity)
        {
            ItemDef? itemDef = ItemManager.GetItemDefById(itemId);

            if (itemDef == null)
            {
                return false;
            }
            Item? sameIdItem = GetItemById(itemId);

            if (sameIdItem != null && (itemDef.Stackable || alwaysStack) && sameIdItem.Amount < int.MaxValue)
            { // If there's an item in the container with the same ID and the item is stackable we can use that slot unless it's amount is maxed
                return true;
            }
            int freeSlots = GetFreeSlots();

            if (freeSlots <= 0)
            { // No free slots
                return false;
            }
            return (itemDef.Stackable || alwaysStack) || freeSlots >= quantity;
        }

        /// <summary>
        /// Attempts to add an existing game item to the container
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="strictQuantity">Whether quantity may be adjusted if there's not enough space available in the container</param>
        /// <returns>The added quantity</returns>
        public int AddItem(Item item, bool strictQuantity = true)
        {
            return AddItem(item.Id, item.Amount, strictQuantity);
        }

        /// <summary>
        /// Attempts to add an existing game item to the container
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="slot">The slot</param>
        /// <param name="strictQuantity">Whether quantity may be adjusted if there's not enough space available in the container</param>
        /// <returns>The added quantity</returns>
        public int AddItem(Item item, int slot, bool strictQuantity = true)
        {
            return AddItemToSlot(item.Id, slot, item.Amount, strictQuantity);
        }

        /// <summary>
        /// Attempts to add an item to the container
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <param name="quantity">The quantity of the item</param>
        /// <param name="strictQuantity">Whether quantity may be adjusted if there's not enough space available in the container</param>
        /// <returns>The added quantity</returns>
        public int AddItem(int itemId, int quantity = -1, bool strictQuantity = false)
        {
            int? availableSlot = GetFirstAvailableSlot(itemId);

            if (availableSlot == null)
            { // No slots available to add the item to
                return 0;
            }
            return AddItemToSlot(itemId, availableSlot.Value, quantity, strictQuantity);
        }

        /// <summary>
        /// Attempts to add an item to a specified slot but fallback to another slot when not successful
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <param name="slot">The slot</param>
        /// <param name="quantity">The quantity of the item</param>
        /// <param name="strictQuantity">Whether quantity may be adjusted if there's not enough space available in the container</param>
        /// <returns>The added quantity</returns>
        public int TryAddItemToSlot(int itemId, int slot, int quantity = -1, bool strictQuantity = false)
        {
            int quantityAdded = AddItemToSlot(itemId, slot, quantity, strictQuantity);

            if (quantityAdded > 0)
            { // Item was at least partially added
                return quantityAdded;
            }
            // Failed to add the item to the defined slot, try to add it without specifying a slot
            return AddItem(itemId, quantity, strictQuantity);
        }

        /// <summary>
        /// Attempts to add an item to a specified slot
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <param name="slot">The slot</param>
        /// <param name="quantity">The quantity of the item</param>
        /// <param name="strictQuantity">Whether quantity may be adjusted if there's not enough space available in the container</param>
        /// <returns>The added quantity</returns>
        public int AddItemToSlot(int itemId, int slot, int quantity = 1, bool strictQuantity = false)
        {
            ItemDef? def = ItemManager.GetItemDefById(itemId);

            if (def == null)
            {
                return 0;
            }
            bool stack = (def.Stackable || alwaysStack);

            Item? itemAtSlot = GetItemBySlot(slot);

            if (stack)
            { // Items can be stacked
                if (itemAtSlot != null && itemAtSlot.Id == itemId)
                { // An item with the same identifier exists at the target slot
                    if (int.MaxValue - itemAtSlot.Amount < quantity)
                    { // The stack would exceed max. value
                        if (strictQuantity)
                        { // We're not allowed to modify the quantity so we can't add any of the items
                            return 0;
                        }
                        quantity = int.MaxValue - itemAtSlot.Amount;
                    }
                    // Increase the quantity of the item in the slot with the quantity to add
                    Items[slot] = new Item(itemAtSlot.Uid, itemAtSlot.Id, quantity);
                    return quantity;
                }
                int? stackableSlot = GetItemSlot(itemId);
                
                if (stackableSlot != null)
                { // Stackable item exists in the container, add to that slot
                    return AddItemToSlot(itemId, stackableSlot.Value, quantity, strictQuantity);
                }
            }
            int slotsRequired = stack ? 1 : quantity;
            int freeSlots = GetFreeSlots();

            if (freeSlots < slotsRequired)
            {
                if (strictQuantity || stack)
                { // Not enough free slots available to add the items
                    return 0;
                }
                quantity = freeSlots;
            }
            for (int i = 0; i < (stack ? 1 : quantity); i++)
            {
                KeyValuePair<int, Item?> freeSlot = Items.FirstOrDefault(kvp => kvp.Value == null);
                Items[freeSlot.Key] = new Item(itemId, stack ? quantity : 1);
            }
            return quantity;
        }

        /// <summary>
        /// Moves an item to another slot
        /// </summary>
        /// <param name="sourceSlot">The source slot</param>
        /// <param name="targetSlot">The target slot</param>
        public void Move(int sourceSlot, int targetSlot)
        {
            Item? sourceItem = GetItemBySlot(sourceSlot);

            if (sourceItem == null)
            { // No source item to move
                return;
            }
            Item? targetItem = GetItemBySlot(targetSlot);
            Items[targetSlot] = sourceItem;
            Items[sourceSlot] = targetItem;
        }

        /// <summary>
        /// Removes items from the container
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <param name="quantity">The quantity to remove</param>
        /// <returns>The modified item if not removed</returns>
        public bool RemoveItems(int itemId, int quantity)
        {
            ItemDef? def = ItemManager.GetItemDefById(itemId);

            if (def == null)
            {
                return false;
            }
            if (def.Stackable || alwaysStack)
            { // Item is stackable
                int? slot = GetItemSlot(itemId);

                if (slot == null)
                { // Failed to find slot for item
                    return false;
                }
                Item? item = Items[slot.Value];

                if (item == null)
                {
                    return false;
                }
                int amount = item.Amount - quantity;

                if (amount <= 0)
                { // Final amount is <= 0, remove
                    Items[slot.Value] = null;
                    return true;
                }
                // Update the amount
                Items[slot.Value] = new Item(item.Uid, item.Id, amount);
                return true;
            }
            for (int i = 0; i < quantity; i++)
            {
                int? slot = GetItemSlot(itemId);

                if (slot == null)
                {
                    break;
                }
                Items[slot.Value] = null;
            }
            return true;
        }
        
        /// <summary>
        /// Removes an item
        /// </summary>
        /// <param name="item">The item</param>
        public void RemoveItem(Item item)
        {
            int? slot = GetItemSlot(item);

            if (slot == null)
            {
                return;
            }
            RemoveItemFromSlot(item, slot.Value);
        }

        /// <summary>
        /// Removes an item from a given slot
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="slot">The slot</param>
        public void RemoveItemFromSlot(Item item, int slot)
        {
            if (!HasItemInSlot(item, slot))
            {
                return;
            }
            Items[slot] = null;
        }

        /// <summary>
        /// Validates the container
        /// </summary>
        public void ValidateContainer()
        { //TODO: Call this at player loading
            List<int> invalidSlots = new();

            foreach (KeyValuePair<int, Item?> kvp in Items)
            {
                if (kvp.Value == null || ItemManager.IsValidItem(kvp.Value, out _))
                {
                    continue;
                }
                invalidSlots.Add(kvp.Key);
            }
            foreach (int slot in invalidSlots)
            {
                Items[slot] = null;
            }
        }

        /// <summary>
        /// Whether an item instance is valid in the container
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>The result</returns>
        public bool IsValidContainerItem(Item? item)
        {
            return item != null && ItemManager.IsValidItem(item, out ItemDef? def)
                && GetItemsById(item.Id).Exists(i => i.Uid == item.Uid);
        }

    }
}
