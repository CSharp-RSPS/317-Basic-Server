using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RSPS.Game.Skills
{
    /// <summary>
    /// Represents a game skill
    /// </summary>
    public sealed class Skill
    {

        /// <summary>
        /// The type of skill
        /// </summary>
        public SkillType SkillType { get; set; }

        /// <summary>
        /// The current absolute level
        /// </summary>
        public int CurrentLevel { get; set; }

        /// <summary>
        /// The visible boost
        /// </summary>
        public int VisibleBoost { get; set; } = 0;

        /// <summary>
        /// The current virtual level
        /// </summary>
        public int VirtualLevel => CurrentLevel + VisibleBoost;

        /// <summary>
        /// The current experience
        /// </summary>
        public int CurrentExperience { get; set; }


        /// <summary>
        /// Creates a new skill
        /// </summary>
        /// <param name="skillType">The skill type</param>
        /// <param name="experience">The current level</param>
        /// <param name="level">The current experience</param>
        public Skill(SkillType skillType, int level = 1, int experience = 0)
        {
            SkillType = skillType;
            CurrentLevel = level;
            CurrentExperience = experience;
        }

    }
}
