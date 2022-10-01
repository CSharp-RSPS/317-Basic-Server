using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items.Equipment
{
    /// <summary>
    /// Handles equipment related operations
    /// </summary>
    public static class EquipmentHandler
    {

        /// <summary>
        /// The names of the possible equipment bonuses
        /// </summary>
        private static readonly string[] BonusNames = { "Stab", "Slash", "Crush",
            "Magic", "Range", "Stab", "Slash", "Crush", "Magic", "Range",
            "Strength", "Prayer" };


        /// <summary>
        /// Attempts to equip an item for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="itemId">The identifier of the item</param>
        /// <param name="slot">The item slot</param>
        public static void Equip(Player player, int itemId, int slot)
        {
            Item? inventoryItem = player.Inventory.GetItemBySlot(slot);

            if (inventoryItem == null || inventoryItem.Id != itemId)
            {
                return;
            }
            //TODO get item type => slot for it
            int equipmentSlot = 0;

            Item? itemAtSlot = player.Equipment.GetItemBySlot(equipmentSlot);

            if (itemAtSlot != null)
            {
                if (itemAtSlot.Id == inventoryItem.Id)
                { // Swap
                    // TODO if stackable - change decrease inv. amount, add equip amount
                    // TODO if not stackable => swap
                }
                // TODO If 2-handed and also has shield - check if enough inventory space - if so, add item (not to slot)
            }
            player.Inventory.RemoveItemFromSlot(inventoryItem, slot);
            player.Equipment.AddItem(inventoryItem, equipmentSlot);

            if (itemAtSlot != null)
            {
                //TODO if stackable and in inventory, add to stack, not the slot
                player.Inventory.AddItem(itemAtSlot, slot);
            }
            player.Inventory.RefreshUI(player);
            WriteBonuses(player);
            UpdateWeight(player);
        }

        /// <summary>
        /// Attempts to un-equip an item for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="itemId">The identifier of the item</param>
        /// <param name="slot">The item slot</param>
        public static void Unequip(Player player, int itemId, int slot)
        {
            Item? item = player.Equipment.GetItemBySlot(slot);

            if (item == null || item.Id != itemId)
            {
                return;
            }
            if (!player.Inventory.CanAddItem(item))
            {
                PacketHandler.SendPacket(player, new SendMessage("You don't have enough space in your inventory."));
                return;
            }
            player.Equipment.RemoveItemFromSlot(item, slot);
            player.Inventory.AddItem(item);
            player.Equipment.RefreshUI(player);
            WriteBonuses(player);
            UpdateWeight(player);
        }

        /// <summary>
        /// Updates the carried weight for a player
        /// </summary>
        /// <param name="player">The player</param>
        public static void UpdateWeight(Player player)
        {
            List<Item> items = new();

            player.Equipment.Items.Values.ToList().ForEach(item => { 
                if (item != null)
                {
                    items.Add(item);
                }
            });
            player.Inventory.Items.Values.ToList().ForEach(item => {
                if (item != null)
                {
                    items.Add(item);
                }
            });
            short weight = 0;

            foreach (Item item in items)
            {
                ItemDef? def = ItemManager.GetItemDefById(item.Id);

                if (def == null)
                {
                    continue;
                }
                weight += def.Weight;
            }
            PacketHandler.SendPacket(player, new SendWeight(weight));
        }

        public static void WriteBonuses(Player player)
        {
            int[] bonuses = new int[player.Equipment.Capacity];

            for (int i = 0; i < player.Equipment.Capacity; i++)
            {
                bonuses[i] += 0;
            }
        }

    }
}
