using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Skills
{
    /// <summary>
    /// Represents a skill requirement
    /// </summary>
    public sealed class SkillRequirement
    {

        /// <summary>
        /// The skill type
        /// </summary>
        public SkillType SkillType { get; set; }

        /// <summary>
        /// The required level
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Whether the requirement is valid when the skill is boosted
        /// </summary>
        public bool BoostAllowed { get; set; }

    }
}
