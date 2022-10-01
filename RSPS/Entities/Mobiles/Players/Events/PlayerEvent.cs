using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Mobiles.Players.Events
{
    /// <summary>
    /// Represents a player action event
    /// </summary>
    public abstract class PlayerEvent : IPlayerEvent
    {

        /// <summary>
        /// The player event type
        /// </summary>
        public PlayerEventType PlayerEventType { get; private set; }


        /// <summary>
        /// Creates a new player action event
        /// </summary>
        /// <param name="playerEventType">The player event type</param>
        protected PlayerEvent(PlayerEventType playerEventType)
        {
            PlayerEventType = playerEventType;
        }

        public abstract bool Execute(Player player);

    }
}
