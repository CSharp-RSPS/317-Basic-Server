using RSPS.Game.Items.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items
{
    /// <summary>
    /// Represents an item definition
    /// </summary>
    public sealed class ItemDef
    {

        /// <summary>
        /// The item identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The item name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The item description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The identity of the item when noted/unnoted
        /// </summary>
        public int ReverseIdentity { get; set; }

        /// <summary>
        /// The weight of the item
        /// </summary>
        public short Weight { get; set; }

        /// <summary>
        /// Whether the item is noted
        /// </summary>
        public bool Noted { get; set; }

        /// <summary>
        /// Whether the item can be stacked
        /// </summary>
        public bool Stackable { get; set; }

        /// <summary>
        /// Whether the item is for members only
        /// </summary>
        public bool Member { get; set; }

        /// <summary>
        /// Whether the item can be shared
        /// </summary>
        public bool Shareable { get; set; }

        /// <summary>
        /// Whether the item is enabled for use
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Whether the item can be consumed
        /// </summary>
        public bool Consumable { get; set; }

        /// <summary>
        /// The equip type of the item
        /// </summary>
        public EquipType EquipType { get; set; }

    }
}
