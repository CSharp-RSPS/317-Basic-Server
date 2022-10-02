using RSPS.Entities.Mobiles.Players.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Skills
{
    /// <summary>
    /// Represents a skill boost definition
    /// </summary>
    public sealed class SkillBoost
    {

        /// <summary>
        /// The skill type
        /// </summary>
        public SkillType SkillType { get; set; }

        /// <summary>
        /// The boost amount
        /// </summary>
        public int Boost { get; set; }

        /// <summary>
        /// Whether the boost can exceed the skill level
        /// </summary>
        public bool CanExceedLevel { get; set; }

    }
}
