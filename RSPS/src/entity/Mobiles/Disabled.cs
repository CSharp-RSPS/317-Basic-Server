using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.Mobiles
{
    /// <summary>
    /// Represents info for a disabled mobile
    /// </summary>
    public sealed class Disabled
    {

        /// <summary>
        /// The amount of game ticks the mobile is disabled for
        /// </summary>
        public int TimeDisabled { get; set; }


        /// <summary>
        /// Creates a new disabled instance
        /// </summary>
        public Disabled() : this(60000) { }

        /// <summary>
        /// Creates a new disabled instance
        /// </summary>
        /// <param name="timeDisabled">The amount of game ticks the mobile is disabled for</param>
        public Disabled(int timeDisabled)
        {
            TimeDisabled = timeDisabled;
        }

        /// <summary>
        /// Handles a game tick
        /// </summary>
        public void OnTick()
        {
            TimeDisabled--;
        }

    }
}
