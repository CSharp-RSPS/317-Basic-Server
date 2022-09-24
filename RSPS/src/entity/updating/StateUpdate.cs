using RSPS.src.entity.flag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.updating
{
    /// <summary>
    /// Represents a state update
    /// </summary>
    public abstract class StateUpdate
    {

        /// <summary>
        /// The update type
        /// </summary>
        public FlagType Type { get; private set; }


        /// <summary>
        /// Creates a new state update
        /// </summary>
        /// <param name="type">The update type</param>
        public StateUpdate(FlagType type)
        {
            Type = type;
        }

    }
}
