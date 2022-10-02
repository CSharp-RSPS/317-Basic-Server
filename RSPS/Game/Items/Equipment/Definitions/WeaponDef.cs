using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items.Equipment.Definitions
{
    /// <summary>
    /// Represents a weapon definition
    /// </summary>
    public sealed class WeaponDef
    {

        /// <summary>
        /// The item identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The weapon attack speed
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// The weapon attack distance
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// The type of weapon
        /// </summary>
        public WeaponType WeaponType { get; set; } = WeaponType.Unarmed;

        /// <summary>
        /// The weapon animations
        /// </summary>
        public WeaponAnimations Animations { get; set; } = new();

    }
}
