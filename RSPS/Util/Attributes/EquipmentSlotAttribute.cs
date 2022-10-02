using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Util.Attributes
{
    /// <summary>
    /// Represents an equipment slot attribute
    /// </summary>
    public sealed class EquipmentSlotAttribute : Attribute
    {

        /// <summary>
        /// The slot
        /// </summary>
        public int Slot { get; private set; }


        /// <summary>
        /// Creates a new equipment slot attribute
        /// </summary>
        /// <param name="slot">The slot</param>
        public EquipmentSlotAttribute(int slot)
        {
            Slot = slot;
        }

    }
}
