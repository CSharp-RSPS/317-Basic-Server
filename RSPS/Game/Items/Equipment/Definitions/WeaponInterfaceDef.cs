using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items.Equipment.Definitions
{
    /// <summary>
    /// Represents a weapon interface definition
    /// </summary>
    public sealed class WeaponInterfaceDef
    {

        /// <summary>
        /// The weapon type
        /// </summary>
        public WeaponType WeaponType { get; set; }

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; set; }

        /// <summary>
        /// The ID of the interface name line
        /// </summary>
        public int NameLineId { get; set; }

        /// <summary>
        /// The special bar ID
        /// </summary>
        public int SpecialBarId { get; set; }

        /// <summary>
        /// The special meter ID
        /// </summary>
        public int SpecialMeterId { get; set; }

        /// <summary>
        /// The widgets
        /// </summary>
        public WeaponWidget[] Widgets { get; set; }

        /// <summary>
        /// Whether the definition is for a ranged weapon
        /// </summary>
        public bool IsRanged => WeaponType == WeaponType.Crossbow
            || WeaponType == WeaponType.Dart
            || WeaponType == WeaponType.Javelin
            || WeaponType == WeaponType.Knife
            || WeaponType == WeaponType.Longbow
            || WeaponType == WeaponType.Shortbow
            || WeaponType == WeaponType.Thrownaxe;

        /// <summary>
        /// Whether the definition is for a magic weapon
        /// </summary>
        public bool IsMagic => WeaponType == WeaponType.Staff;

        /// <summary>
        /// Whether the definition is for a melee weapon
        /// </summary>
        public bool IsMelee => !IsMagic && !IsRanged;


        /// <summary>
        /// Creates a new weapon interface definition
        /// </summary>
        /// <param name="weaponType">The weapon type</param>
        /// <param name="interfaceId">The interface ID</param>
        /// <param name="nameLineId">The interface name line ID</param>
        /// <param name="specialBarId">The special bar ID</param>
        /// <param name="specialMeterId">The special meter ID</param>
        /// <param name="widgets">The widgets</param>
        public WeaponInterfaceDef(WeaponType weaponType, int interfaceId, int nameLineId,
            int specialBarId, int specialMeterId, WeaponWidget[] widgets)
        {
            WeaponType = weaponType;
            InterfaceId = interfaceId;
            NameLineId = nameLineId;
            SpecialBarId = specialBarId;
            SpecialMeterId = specialMeterId;
            Widgets = widgets;
        }

        /// <summary>
        /// Retrieves the weapon widget by it's button ID
        /// </summary>
        /// <param name="buttonId">The button ID</param>
        /// <returns>The weapon widget</returns>
        public WeaponWidget? GetWeaponWidgetByButtonId(int buttonId)
        {
            return Widgets?.FirstOrDefault(ww => ww.ButtonId == buttonId);
        }

    }
}
