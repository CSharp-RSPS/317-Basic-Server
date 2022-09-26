using RSPS.src.entity.movement.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src
{
    /// <summary>
    /// Holds general application configurations
    /// </summary>
    public static class Constants
    {

        /// <summary>
        /// The default world tick time in milliseconds
        /// </summary>
        public static readonly int WorldCycleMs = 600;

        /// <summary>
        /// The max. amount of simultaneous connection allowed to the game
        /// </summary>
        public static readonly int MaxSimultaneousConnections = 99999;

        /// <summary>
        /// The default starting position for a player
        /// </summary>
        public static readonly Position PlayerStartingPosition = new(3222, 3222);

    }
}
