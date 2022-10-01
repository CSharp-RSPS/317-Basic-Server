using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// The interface ID of the container
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// The capacity of the container
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// Whether the container is for a member
        /// </summary>
        public bool Member { get; private set; }

        /// <summary>
        /// Whether to always stack items with the same identifier
        /// </summary>
        public bool AlwaysStack { get; private set; }

        /// <summary>
        /// Holds the items in the container
        /// </summary>
        public Dictionary<int, Item?> Items { get; private set; }


        /// <summary>
        /// Creates a new item container
        /// </summary>
        /// <param name="capacity">The capacity</param>
        /// <param name="member">Whether the container is for a member</param>
        /// <param name="alwaysStack">Whether to always stack items with the same identifier</param>
        public ItemContainer(int interfaceId, int capacity, bool member, bool alwaysStack)
        {
            InterfaceId = interfaceId;
            Capacity = capacity;
            Member = member;
            AlwaysStack = alwaysStack;
            Items = new Dictionary<int, Item?>(Capacity);

            for (int i = 0; i < Capacity; i++)
            { // Initialize the container
                Items.Add(i, null);
            }
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
        /// Modifies the amount of an item by a modifier
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="slot">The slot of the item</param>
        /// <param name="modifier">The modifier</param>
        /// <returns>The modified item</returns>
        public Item? ModifyItemAmount(Item item, int slot, int modifier)
        {
            if (int.MaxValue - item.Amount < modifier)
            { // Would exceed max. amount
                return null;
            }
            return SetItemAmount(item, slot, item.Amount + modifier);
        }

        /// <summary>
        /// Modifies the amount of an item
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="slot">The item slot</param>
        /// <param name="amount">The new amount</param>
        /// <returns>The modified item</returns>
        public Item? SetItemAmount(Item item, int slot, int amount)
        {
            Item? itemAtSlot = GetItemBySlot(slot);

            if (itemAtSlot == null || !ItemManager.IsValidItem(item, out ItemDef? def) || def == null
                || itemAtSlot.Uid != item.Uid)
            {
                return null;
            }
            if (amount <= 0)
            { // Remove the item
                Items[slot] = null;
                return null;
            }
            if (amount > 1 && !def.Stackable)
            { // Invalid amount
                return itemAtSlot;
            }
            Items[slot] = new(itemAtSlot.Uid, itemAtSlot.Id, amount);
            return Items[slot];
        }
        
        /// <summary>
        /// Adds a new item, in case of stackable items the amount of the existing item will increase if one exists
        /// </summary>
        /// <param name="item">The item</param>
        /// <param name="slot">The slot</param>
        /// <returns>The added item</returns>
        public Item? AddItem(Item item, int? slot = null)
        {
            if (slot == null)
            {
                slot = GetFirstAvailableSlotForItem(item);

                if (slot == null)
                {
                    return null;
                }
            }
            else if (!CanAddItemToSlot(item, slot.Value))
            { //TODO: we should check for stackable first before trying to force to the slot
                return null;
            }
            ItemDef? def = ItemManager.GetItemDefById(item.Id);

            if (def == null)
            { // Shouldn't ever happen here
                return null;
            }
            if (def.Stackable)
            {
                Item? itemAtSlot = GetItemBySlot(slot.Value);

                if (itemAtSlot != null && itemAtSlot.Id == item.Id)
                {
                    return ModifyItemAmount(itemAtSlot, slot.Value, item.Amount);
                }
                Item? stack = GetItemById(item.Id);

                if (stack != null)
                {
                    int? stackSlot = GetItemSlot(stack);

                    if (stackSlot != null)
                    {
                        return ModifyItemAmount(stack, stackSlot.Value, item.Amount);
                    }
                }
            }
            Items[slot.Value] = item;
            return item;
        }

        /// <summary>
        /// Retrieves whether an item can be added to the container
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>The result</returns>
        /// <exception cref="InvalidDataException"></exception>
        public bool CanAddItem(Item item)
        {
            return GetFirstAvailableSlotForItem(item) != null;
        }

        /// <summary>
        /// Retrieves the first available slot for an item
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>The slot</returns>
        public int? GetFirstAvailableSlotForItem(Item item)
        {
            for (int slot = 0; slot < Capacity; slot++)
            {
                if (CanAddItemToSlot(item, slot))
                {
                    return slot;
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieves whether an item can be added to a specific slot
        /// </summary>
        /// <param name="item">the item</param>
        /// <param name="slot">The slot</param>
        /// <returns>The result</returns>
        private bool CanAddItemToSlot(Item item, int slot)
        {
            if (slot < 0 || slot >= Capacity)
            {
                return false;
            }
            if (!ItemManager.IsValidItem(item, out ItemDef? def) || def == null)
            {
                return false;
            }
            if (def.Member && !Member)
            {
                return false;
            }
            Item? itemInSlot = GetItemBySlot(slot);

            return itemInSlot == null
                || itemInSlot.Id == item.Id && (def.Stackable || AlwaysStack);
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
        /// Retrieves the slot in the container an item is in
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>The slot</returns>
        public int? GetItemSlot(Item item)
        {
            foreach (KeyValuePair<int, Item?> kvp in Items)
            {
                if (kvp.Value != null && kvp.Value.Uid == item.Uid)
                {
                    return kvp.Key;
                }
            }
            return -1;
        }

        /// <summary>
        /// Retreives whether an item is present in a given slot
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
        /// Retrieves whether an item with a given identifier is present in a slot
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <param name="slot">The slot</param>
        /// <returns>The result</returns>
        public bool HasItemInSlot(int id, int slot)
        {
            Item? itemInSlot = GetItemBySlot(slot);
            return itemInSlot != null && itemInSlot.Id == id;
        }
        
        /// <summary>
        /// Retrieves whether an item with a given identifier is present
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>The result</returns>
        public bool HasItem(int id)
        {
            return GetItemById(id) != null;
        }

        /// <summary>
        /// Retrieves whether a specified amount of items with a given identifier is present
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <param name="amount">The amount</param>
        /// <returns>The result</returns>
        public bool HasAmountOfItem(int id, int amount)
        {
            return GetAmountOfItem(id) >= amount;
        }

        /// <summary>
        /// Retrieves the amount of items present with a given identifier
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>The result</returns>
        public int GetAmountOfItem(int id)
        {
            return GetItemsById(id).Sum(item => item.Amount);
        }

        /// <summary>
        /// Retrieves the items in the container with a given identifier
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>The items</returns>
        public List<Item> GetItemsById(int id)
        {
            List<Item> items = new();

            foreach (Item? item in Items.Values)
            {
                if (item != null && item.Id == id)
                {
                    items.Add(item);
                }
            }
            return items;
        }

        /// <summary>
        /// Retrieves the first item found in the container with a given identifier
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>The item</returns>
        public Item? GetItemById(int id)
        {
            return Items.Values.FirstOrDefault(item => item != null && item.Id == id);
        }

        /// <summary>
        /// Retrieves an item by it's slot
        /// </summary>
        /// <param name="slot">The slot</param>
        /// <returns>The item</returns>
        public Item? GetItemBySlot(int slot)
        {
            return !Items.ContainsKey(slot) ? null : Items[slot];
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
        /// Validates the container
        /// </summary>
        public void ValidateContainer()
        { //TODO: Call this at player loading
            List<int> invalidSlots = new();

            foreach (KeyValuePair<int, Item?> kvp in Items)
            {
                if (kvp.Value == null || ItemManager.IsValidItem(kvp.Value, out ItemDef? def))
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

        /// <summary>
        /// Retrieves the free slots in the container
        /// </summary>
        /// <returns>The free slots</returns>
        public int GetFreeSlots()
        {
            int freeSlots = 0;

            foreach (KeyValuePair<int, Item?> kvp in Items)
            {
                if (kvp.Value == null)
                {
                    freeSlots++;
                }
            }
            return freeSlots;
        }

        /// <summary>
        /// Refreshes the UI for a player with the items within this container
        /// </summary>
        /// <param name="player">The player</param>
        /// <returns>The container</returns>
        public ItemContainer RefreshUI(Player player)
        {
            PacketHandler.SendPacket(player, new SendDrawItemsOnInterface2(InterfaceId, Items));
            return this;
        }

    }
}
