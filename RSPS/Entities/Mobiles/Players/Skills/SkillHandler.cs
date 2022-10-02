using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Mobiles.Players.Skills
{
    public static class SkillHandler
    {

        public static int CalculateCombatLevel(Player player)
        {
            int defence = player.GetSkill(SkillType.Defence).CurrentLevel;
            int hitpoints = player.GetSkill(SkillType.Hitpoints).CurrentLevel;
            int prayer = player.GetSkill(SkillType.Prayer).CurrentLevel;
            double baseCombatLevel = 0.25 * (defence + hitpoints + (prayer * 0.5));

            int meleeCombatLevel = (int)Math.Floor(baseCombatLevel + 0.325 * 
                (player.GetSkill(SkillType.Attack).CurrentLevel + player.GetSkill(SkillType.Strength).CurrentLevel));

            int rangeCombatLevel = (int)Math.Floor(baseCombatLevel + 0.325 * (player.GetSkill(SkillType.Ranged).CurrentLevel * 1.5));
            int magicCombatLevel = (int)Math.Floor(baseCombatLevel + 0.325 * (player.GetSkill(SkillType.Magic).CurrentLevel * 1.5));

            int combatLevel = Math.Max(meleeCombatLevel, Math.Max(rangeCombatLevel, magicCombatLevel));
            return combatLevel > 3 ? combatLevel : 3;

        }

        public static int GetTotalLevel(Player player)
        {
            int amount = 0;
            foreach(SkillType type in Enum.GetValues(typeof(SkillType)))
            {
                amount += player.GetSkill(type).CurrentLevel;
            }
            return amount;
        }

        private static int GetLevelFromExperience(double experience)
        {

            int level = 0;

            for (int lvl = 1; lvl <= 99; lvl++)
            {
                double levelPower = Math.Pow(2, (double)(lvl - 1) / 7);
                double experienceToLevel = Math.Floor(0.25 * (lvl - 1 + (300 * (levelPower))));
                level = (int)experienceToLevel;
                
                if (level >= experience)
                {
                    return level;
                }
            }
            return 99;
        }

        private static double CalculateExperienceForNextLevel(int nextLevel)
        {
            double levelPower = Math.Pow(2, (double)(nextLevel - 1) / 7);
            double experienceToLevel = Math.Floor(0.25 * (nextLevel - 1 + (300 * (levelPower))));
            return experienceToLevel;
        }

    }
}
