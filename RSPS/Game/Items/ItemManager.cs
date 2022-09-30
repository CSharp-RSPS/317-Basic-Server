using RSPS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items
{
    /// <summary>
    /// Manages item related operations
    /// </summary>
    public static class ItemManager
    {

        /// <summary>
        /// Holds the item definitions
        /// </summary>
        public static readonly Dictionary<int, ItemDef> Definitions = new();

        
        static ItemManager()
        {
            JsonUtil.DataImport<ItemDef>("./Resources/items/item_information.json", 
                (elements) => elements.ForEach(def => Definitions.Add(def.Identity, def)));
        }

        /// <summary>
        /// Retrieves an item definition for an identifier
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>The item definition</returns>
        public static ItemDef? GetItemDefById(int id)
        {
            return Definitions.ContainsKey(id) ? Definitions[id] : null;
        }

        /// <summary>
        /// Retrieves whether an item identifier is valid to the game
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <param name="def">The item definition to use if present</param>
        /// <returns>The result</returns>
        public static bool IsValidItemId(int id)
        {
            if (id < 0)
            {
                return false;
            }
            ItemDef? def = GetItemDefById(id);
            return def != null && def.Enabled;
        }

        /// <summary>
        /// Whether an item instance is valid
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>The result</returns>
        public static bool IsValidItem(Item item, out ItemDef? def)
        {
            if (item.Id < 0 || item.Amount <= 0 || string.IsNullOrEmpty(item.Uid))
            {
                def = null;
                return false;
            }
            def = GetItemDefById(item.Id);
            return def != null && def.Enabled && (item.Amount == 1 || def.Stackable);
        }

    }
}
