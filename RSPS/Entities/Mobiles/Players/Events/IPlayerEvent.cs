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
    public interface IPlayerEvent
    {


        /// <summary>
        /// Whether execution finished
        /// </summary>
        /// <param name="player">The player</param>
        /// <returns>The result</returns>
        public bool Execute(Player player);

    }
}
