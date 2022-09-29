using RSPS.src.entity.Health;
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
    public sealed class CombatableNpc : Npc, ICombatable
    {


        public CombatableNpc(NpcSpawn spawn) : base(spawn)
        {

        }

        public dynamic Combat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public dynamic FightType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoRetialate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Hitpoints Hitpoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
