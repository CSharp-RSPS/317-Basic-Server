using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.Mobiles.Npcs.Definitions
{
    /// <summary>
    /// Represents an NPC definition
    /// </summary>
    public sealed class NpcDefinition
    {

        /// <summary>
        /// The name of the NPC
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The description of the NPC
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The NPC identifier
        /// </summary>
        public int Identity { get; set; }

        /// <summary>
        /// The hitbox size of the NPC
        /// </summary>
        public int Size { get; set; }

    }
}
