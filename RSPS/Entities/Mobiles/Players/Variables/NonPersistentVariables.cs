using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Mobiles.Players.Variables
{
    /// <summary>
    /// Holds non-persistent player variables that are used globally
    /// </summary>
    public sealed class NonPersistentVariables
    {

        /// <summary>
        /// The identifier of the interface the player has open, if any
        /// </summary>
        public int OpenInterfaceId { get; set; } = -1;

        /// <summary>
        /// Whether the player has an interface opened
        /// </summary>
        public bool HasOpenInterface => OpenInterfaceId != -1;

        /// <summary>
        /// Whether to withdraw items from the bank noted
        /// </summary>
        public bool NotedBanking { get; set; }

    }
}
