using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RSPS.src.entity.Mobiles.Players.Skills
{
    public struct Skill
    {

        public SkillType SkillType { get; set; }
        public int CurrentLevel { get; set; }
        public int VisibleBoost { get; set; } = 0;//max level for maybe temporary skill boost
        public int CurrentExperience { get; set; }

        public Skill(SkillType skillType)
        {
            SkillType = skillType;
            CurrentLevel = 1;
            CurrentExperience = 0;
        }

        public Skill(SkillType skillType, int level, int experience)
        {
            SkillType = skillType;
            CurrentLevel = level;
            CurrentExperience = experience;
        }

    }
}
