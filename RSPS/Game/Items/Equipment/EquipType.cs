using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items.Equipment
{
    /// <summary>
    /// The possible equip types
    /// </summary>
    public enum EquipType
    {

        [EquipmentSlot(0)]
        Head = 0,

        [EquipmentSlot(1)]
        Cape = 1,

        [EquipmentSlot(2)]
        Amulet = 2,

        [EquipmentSlot(3)]
        Weapon = 3,

        [EquipmentSlot(4)]
        Chest = 4,

        [EquipmentSlot(5)]
        Shield = 5,

        [EquipmentSlot(3)]
        None = 6,

        [EquipmentSlot(7)]
        Legs = 7,

        [EquipmentSlot(3)]
        Empty = 8,

        [EquipmentSlot(9)]
        Hands = 9,

        [EquipmentSlot(10)]
        Feet = 10,

        [EquipmentSlot(11)]
        Ring = 11,

        [EquipmentSlot(12)]
        Ammo = 12,

        [EquipmentSlot(3)]
        Empty2 = 13

    }
}
