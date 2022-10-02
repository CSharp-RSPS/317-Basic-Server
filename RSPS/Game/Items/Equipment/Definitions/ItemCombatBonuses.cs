using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items.Equipment.Definitions
{
    /// <summary>
    /// Holds item combat bonuses
    /// </summary>
    public sealed class ItemCombatBonuses
    {

        public int StabAttackBonus { get; set; }

        public int SlashAttackBonus { get; set; }

        public int CrushAttackBonus { get; set; }

        public int RangedAttackBonus { get; set; }

        public int MagicAttackBonus { get; set; }

        public int StabDefenceBonus { get; set; }

        public int SlashDefenceBonus { get; set; }

        public int CrushDefenceBonus { get; set; }

        public int RangedDefenceBonus { get; set; }

        public int MagicDefenceBonus { get; set; }

        public int StrengthBonus { get; set; }

        public int PrayerBonus { get; set; }

    }
}
