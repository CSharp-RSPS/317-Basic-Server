using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Mobiles.Players.Variables
{
    /// <summary>
    /// Holds persistent player variables that are used globally
    /// </summary>
    public sealed class PersistentVariables
    {

        /// <summary>
        /// The administrative rights of the player
        /// </summary>
        public PlayerRights Rights { get; set; } = PlayerRights.Administrator;

        /// <summary>
        /// Whether the player is a member
        /// </summary>
        public bool Member { get; set; } = true;

        /// <summary>
        /// Whether the player is flagged
        /// </summary>
        public bool Flagged { get; set; }

        /// <summary>
        /// The current energy level for special attacks
        /// </summary>
        public int SpecialEnergy { get; set; }

    }
}
