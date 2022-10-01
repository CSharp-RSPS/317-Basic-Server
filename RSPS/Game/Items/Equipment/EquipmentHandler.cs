using RSPS.Entities.Mobiles.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items.Equipment
{
    /// <summary>
    /// Handles equipment related operations
    /// </summary>
    public static class EquipmentHandler
    {

        /// <summary>
        /// The names of the possible equipment bonuses
        /// </summary>
        private static readonly string[] BonusNames = { "Stab", "Slash", "Crush",
            "Magic", "Range", "Stab", "Slash", "Crush", "Magic", "Range",
            "Strength", "Prayer" };

        public static void WriteBonuses(Player player)
        {
            int[] bonuses = new int[player.Equipment.Capacity];

            for (int i = 0; i < player.Equipment.Capacity; i++)
            {
                bonuses[i] += 0;
            }
        }

    }
}
