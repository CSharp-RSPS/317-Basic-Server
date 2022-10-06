using RSPS.Entities.Mobiles;
using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.movement.Locations;
using RSPS.Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Items.GroundItems
{
    /// <summary>
    /// Represents a ground item
    /// </summary>
    public sealed class GroundItem : Entity
    {

        /// <summary>
        /// The ground item spawn definition in case of a spawned item
        /// </summary>
        public GroundItemSpawn? Spawn { get; private set; }

        /// <summary>
        /// The item identifier
        /// </summary>
        public int ItemId { get; private set; }

        /// <summary>
        /// The quantity of the item
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The lifetime of the ground item
        /// </summary>
        public int Lifetime { get; set; }


        /// <summary>
        /// Creates a new ground item based on a spawn
        /// </summary>
        /// <param name="spawn">The spawn</param>
        public GroundItem(GroundItemSpawn spawn)
            : this(spawn.Position, spawn.Item.Id, spawn.Item.Amount, spawn.Lifetime) {
            Spawn = spawn;
        }

        /// <summary>
        /// Creates a new ground item
        /// </summary>
        /// <param name="position">The position of the item in the game world</param>
        /// <param name="itemId">The item identifier</param>
        /// <param name="quantity">The item quantity</param>
        /// <param name="lifetime">The amount of game ticks the ground item stays available or takes to respawn</param>
        /// <param name="owner">The owner of the item when applicable</param>
        public GroundItem(Position position, int itemId, int quantity, int lifetime, Mobile? owner = null)
            : base(position)
        {
            ItemId = itemId;
            Quantity = quantity;
            Lifetime = lifetime;
            Owner = owner;
        }

    }
}
