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
    /// Represents a ground item spawn definition
    /// </summary>
    public sealed class GroundItemSpawn
    {

        /// <summary>
        /// The item to spawn
        /// </summary>
        public Item Item { get; set; } = new(0);

        /// <summary>
        /// The spawn position
        /// </summary>
        public Position Position { get; set; } = new(0, 0);

        /// <summary>
        /// The re-spawn time
        /// </summary>
        public int Lifetime { get; set; }

        /// <summary>
        /// Whether the spawn has been loaded
        /// </summary>
        public bool Loaded { get; set; }

    }
}
