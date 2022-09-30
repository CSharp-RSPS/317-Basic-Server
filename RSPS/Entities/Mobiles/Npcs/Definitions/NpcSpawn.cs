using RSPS.Entities.movement.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Mobiles.Npcs.Definitions
{
    /// <summary>
    /// Represents an NPC spawn definition
    /// </summary>
    public sealed class NpcSpawn
    {

        /// <summary>
        /// The NPC identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The spawn point of the NPC
        /// </summary>
        public Position SpawnPoint { get; set; } = new Position(0, 0, 0);

        /// <summary>
        /// The NPC's movement area
        /// </summary>
        public Area? MovementArea { get; set; } = null;

        /// <summary>
        /// The NPC's rotation
        /// </summary>
        public int Rotation { get; set; } = -1;

        /// <summary>
        /// Whether the npc is currently spawned
        /// </summary>
        public bool Spawned { get; set; }

    }
}
