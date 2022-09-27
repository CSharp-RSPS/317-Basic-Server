using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity
{
    /// <summary>
    /// Represents constitution for an entity
    /// </summary>
    public sealed class Constitution
    {

        /// <summary>
        /// The default health level
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// The current health level
        /// </summary>
        public int CurrentHealth { get; set; }


        /// <summary>
        /// Modifies the health and retrieves the new health level
        /// </summary>
        /// <param name="modifier">The health modifier</param>
        /// <returns>The new health level</returns>
        public int ModifyHealth(int modifier)
        {
            CurrentHealth += modifier;

            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }
            return CurrentHealth;
        }

    }
}
