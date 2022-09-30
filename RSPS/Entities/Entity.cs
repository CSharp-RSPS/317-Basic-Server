using RSPS.Entities.Mobiles;
using RSPS.Entities.movement.Locations;
using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.Updating.flag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities
{
    /// <summary>
    /// Represents an entity in the game world
    /// </summary>
    public abstract class Entity
    {

        /// <summary>
        /// The player nearby the entity
        /// </summary>
        public List<Player> LocalPlayers { get; private set; }

        /// <summary>
        /// The owner of the entity if any
        /// </summary>
        public Mobile? Owner { get; set; }

        /// <summary>
        /// Whether the entity has an owner
        /// </summary>
        public bool HasOwner => Owner != null;

        /// <summary>
        /// The current position of the entity
        /// </summary>
        public Position Position { get; private set; }

        
        /// <summary>
        /// Creates a new entity
        /// </summary>
        /// <param name="position">The position of the entity</param>
        public Entity(Position position)
        {
            Position = position;
            LocalPlayers = new();
        }

    }
}
