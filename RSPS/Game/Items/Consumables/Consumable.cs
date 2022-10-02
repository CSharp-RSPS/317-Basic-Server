using RSPS.Entities.Mobiles;
using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items.Consumables
{
    /// <summary>
    /// Represents a consumable
    /// </summary>
    public class Consumable : IConsumable
    {

        /// <summary>
        /// The consumable item identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The item identifier of the item that is left after consuming the consumable
        /// </summary>
        public int NextId { get; set; } = -1;

        /// <summary>
        /// The skills boosted by consuming the item
        /// </summary>
        public SkillBoost[] Boosts { get; set; } = Array.Empty<SkillBoost>();


        public bool Consume<T>(T mob) where T : Mobile {
            if (NextId > -1)
            {

            }
            if (Boosts.Length > 0)
            {
                foreach (SkillBoost skillBoost in Boosts)
                {

                }
            }
            return true;
        }

    }
}
