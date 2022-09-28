using RSPS.src.entity.Mobiles.Npcs.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.Mobiles.Npcs.Combat
{
    /// <summary>
    /// Represents an NPC that can engage in combat
    /// </summary>
    public sealed class CombatableNpc : Npc
    {


        public CombatableNpc(NpcSpawn spawn) : base(spawn)
        {

        }

    }
}
