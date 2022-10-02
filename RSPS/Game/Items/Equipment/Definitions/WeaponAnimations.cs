using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items.Equipment.Definitions
{
    /// <summary>
    /// Holds weapon animations
    /// </summary>
    public sealed class WeaponAnimations
    {

        /// <summary>
        /// The standing animation ID
        /// </summary>
        public int Stand { get; set; }

        /// <summary>
        /// The walking animation ID
        /// </summary>
        public int Walk { get; set; }

        /// <summary>
        /// The running animation ID
        /// </summary>
        public int Run { get; set; }

        /// <summary>
        /// The hit animation ID
        /// </summary>
        public int Hit { get; set; }

        /// <summary>
        /// The blocking animation ID
        /// </summary>
        public int Block { get; set; }

    }
}
