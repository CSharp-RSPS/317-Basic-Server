using RSPS.src.entity.movement.Locations;
using RSPS.src.game.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.Items.GroundItems
{
    /// <summary>
    /// Represents a ground item
    /// </summary>
    public sealed class GroundItem : Entity
    {

        /// <summary>
        /// The item on the ground
        /// </summary>
        public Item Item { get; private set; }

        /// <summary>
        /// Whether the ground item can respawn
        /// </summary>
        public bool Respawnable { get; private set; }

        /// <summary>
        /// The amount of game ticks the ground item stays available
        /// </summary>
        public int LifeTime { get; private set; }


        /// <summary>
        /// Creates a new ground item
        /// </summary>
        /// <param name="item">The item on the ground</param>
        /// <param name="respawnable">Whether the ground item can respawn</param>
        /// <param name="lifetime">The amount of game ticks the ground item stays available</param>
        public GroundItem(Item item, bool respawnable, int lifetime)
            : base(new Position(0, 0)) //TODO
        {
            Item = item;
            Respawnable = respawnable;
            LifeTime = lifetime;
        }

    }
}
