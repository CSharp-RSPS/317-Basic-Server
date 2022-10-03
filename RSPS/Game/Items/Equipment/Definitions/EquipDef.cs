using RSPS.Game.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items.Equipment.Definitions
{
    /// <summary>
    /// Represents an equipment definition
    /// </summary>
    public sealed class EquipDef
    {

        public int Id { get; set; }

        public bool TwoHanded { get; set; }

        public bool Platebody { get; set; }

        public bool FullHelm { get; set; }

        public EquipType EquipType { get; set; } = EquipType.None;

        public ItemCombatBonuses Bonuses { get; set; } = new();

        public SkillRequirement[] SkillRequirements { get; set; } = Array.Empty<SkillRequirement>();

        public bool HasSkillRequirements => SkillRequirements != null && SkillRequirements.Length > 0;

    }
}
