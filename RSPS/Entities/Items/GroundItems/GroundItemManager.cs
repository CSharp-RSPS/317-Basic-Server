using RSPS.Data;
using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.movement.Locations;
using RSPS.Entities.movement.Locations.Regions;
using RSPS.Game.Items;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Items.GroundItems
{
    /// <summary>
    /// Manages ground item entities
    /// </summary>
    public sealed class GroundItemManager : EntityManager<GroundItem>
    {

        /// <summary>
        /// Holds the ground-item spawns
        /// </summary>
        private static readonly List<GroundItemSpawn> Spawns = new();

        static GroundItemManager()
        {
       //TODO item in file to itemId, amount     JsonUtil.DataImport<GroundItemSpawn>("./Resources/items/global_ground_items.json", (spawns) => spawns.ForEach(spawn => Spawns.Add(spawn)));
        }

        /// <summary>
        /// Finds a ground item based on their item identifier and position
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <param name="position">The position</param>
        /// <returns>The ground item</returns>
        public GroundItem? FindGroundItem(int itemId, Position position)
        {
            return Entities.FirstOrDefault(gi => gi.ItemId == itemId && gi.Position.Equals(position));
        }

        /// <summary>
        /// Loads the ground items in a region
        /// </summary>
        /// <param name="region">The region</param>
        public void LoadRegionGroundItems(Region region)
        {
            foreach (GroundItemSpawn spawn in Spawns.Where(s => !s.Loaded))
            {
                int regionId = RegionManager.GetRegionId(spawn.Position);

                if (region.Id != regionId)
                {
                    continue;
                }
                spawn.Loaded = true;

                GroundItem gi = new(spawn) { 
                    Lifetime = -1
                };
                Add(gi);

                UpdateGroundItem(gi, true);
            }
        }

        /// <summary>
        /// Attempts to pickup a ground item for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="itemId">The item identifier</param>
        /// <param name="position">The item position</param>
        public void PickupGroundItem(Player player, int itemId, Position position)
        {
            GroundItem? gi = FindGroundItem(itemId, position);

            if (gi == null)
            {
                return;
            }
            //TODO: Walk-to event

            if (!player.Inventory.HasRoomForItems(gi.ItemId, gi.Quantity))
            {
                return;
            }
            int added = player.Inventory.AddItem(gi.ItemId, gi.Quantity);
            RemoveGroundItems(gi, added);
        }

        /// <summary>
        /// Removes ground items
        /// </summary>
        /// <param name="groundItem">The ground item</param>
        /// <param name="quantity">The quantity to remove</param>
        private void RemoveGroundItems(GroundItem groundItem, int quantity)
        {
            // Modify the quantity
            groundItem.Quantity -= quantity;

            if (groundItem.Spawn != null)
            { // If the item is respawnable, start the respawn timer
                groundItem.Lifetime = 300;
            }
            if (quantity > 0)
            { // Add the modified ground item to the view of the players
                groundItem.LocalPlayers.ForEach(p => PacketHandler.SendPacket(p, 
                    new SendEditGroundItemAmount()));
            }
            else if (quantity <= 0)
            { // Remove the ground item from the view of the players
                if (groundItem.Spawn == null)
                { // Remove the ground item completely from the game world
                    Remove(groundItem);
                }
                groundItem.LocalPlayers.ForEach(p => PacketHandler.SendPacket(p,
                   new SendDeleteGroundItem()));
            }
        }

        public void DropItem(Player player, int itemId, int slot, bool removeFromInventory = true)
        {
            Item? item = player.Inventory.GetItemBySlot(slot);

            if (item == null || item.Id != itemId)
            {
                return;
            }
            ItemDef? itemDef = ItemManager.GetItemDefById(itemId);

            if (itemDef == null)
            {
                return;
            }
            if (!itemDef.Shareable || (itemDef.Member && !player.PersistentVars.Member))
            {

            }
            if (removeFromInventory)
            {
                player.Inventory.RemoveItemFromSlot(item, slot);
            }
            GroundItem gi = new(player.Position.Copy(), item.Id, item.Amount, 300, player);
            Add(gi);

            
        }

        private void SendGroundItem(Player player, GroundItem gi)
        {
            PacketHandler.SendPacket(player, new SendSetLocalPlayerCoordinates(player.Position.LocalX, player.Position.LocalY));
            PacketHandler.SendPacket(player, new SendGroundItem(gi.ItemId, gi.Quantity));
        }

        private void SendRemoveGroundItem(Player player, GroundItem gi)
        {
            PacketHandler.SendPacket(player, new SendSetLocalPlayerCoordinates(player.Position.LocalX, player.Position.LocalY));
            PacketHandler.SendPacket(player, new SendRemoveNonSpecifiedGroundItem(gi.ItemId));
        }

        /// <summary>
        /// Spawns a ground-item for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="item">The item</param>
        /// <param name="position">The position</param>
        /// <param name="lifetime">The ground-item lifetime</param>
        public void SpawnGroundItem(Player player, Item item, Position position, int lifetime)
        {
            GroundItem? itemAtPos = Entities.FirstOrDefault(gi => gi.ItemId == item.Id && gi.Position.Equals(position));
            GroundItem create = new(position, item.Id, item.Amount, lifetime, player);

            if (itemAtPos != null)
            {
                ItemDef? itemDef = ItemManager.GetItemDefById(item.Id);

                if (itemDef == null)
                {
                    return;
                }
                if (itemDef.Stackable)
                { //TODO: through processing, once visible, add to existing visible stack (if any)

                }
                if (itemAtPos.Owner == player)
                {
                    create.Quantity += itemAtPos.Quantity;
                    UpdateGroundItem(itemAtPos, false);
                    Remove(itemAtPos);
                }
            }
            Add(create);
            UpdateGroundItem(create, true);
        }

        /// <summary>
        /// Updates the visbility of a ground-item
        /// </summary>
        /// <param name="gi">The ground-item</param>
        /// <param name="visible">Whether the ground item should be visible</param>
        private void UpdateGroundItem(GroundItem gi, bool visible)
        {
            foreach (Player player in WorldHandler.World.Players.Entities)
            {
                if (player.Position.IsWithinDistance(gi.Position, 40))
                {
                    if (!gi.LocalPlayers.Contains(player))
                    {
                        gi.LocalPlayers.Add(player);
                    }
                    continue;
                }
                if (gi.LocalPlayers.Contains(player))
                {
                    gi.LocalPlayers.Remove(player);
                }
            }
            foreach (Player player in gi.LocalPlayers)
            {
                PacketHandler.SendPacket(player, visible 
                    ? new SendGroundItem(gi.ItemId, gi.Quantity) 
                    : new SendDeleteGroundItem());
            }
        }

        /// <summary>
        /// Updates the ground-items in the area for a player
        /// </summary>
        /// <param name="player">The player</param>
        public void UpdateGroundItems(Player player)
        {
            foreach (GroundItem gi in Entities.Where(gi => 
                (gi.Owner == null || gi.Owner == player)
                && (gi.Spawn == null || gi.Lifetime == -1)
                && (gi.Position.IsWithinDistance(player.Position, 40))))
            {
                PacketHandler.SendPacket(player, new SendGroundItem(gi.ItemId, gi.Quantity));
            }
        }

        public override void PrepareTick(GroundItem gi)
        {
            if (gi.Lifetime > 0)
            {
                gi.Lifetime--;
            }
            if (gi.Spawn != null)
            {
                if (gi.Lifetime == 0)
                {
                    gi.Lifetime = -1;
                    UpdateGroundItem(gi, true);
                }
                return;
            }
            if (gi.Owner != null && gi.Lifetime == 200)
            {
                gi.Owner = null;
                UpdateGroundItem(gi, true);
            }
            if (gi.Lifetime == 0)
            {
                UpdateGroundItem(gi, false);
                Remove(gi);
            }
        }

        public override void FinishTick(GroundItem gi)
        {

        }

    }
}
