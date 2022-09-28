using RSPS.src.entity.Mobiles.Npcs.Definitions;
using RSPS.src.entity.movement.Locations;
using RSPS.src.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.Mobiles.Npcs.Pets
{
    /// <summary>
    /// Represents a mobile pet
    /// </summary>
    public sealed class Pet : Npc
    {


        /// <summary>
        /// Creates a new pet
        /// </summary>
        /// <param name="id">The pet' NPC identifier</param>
        /// <param name="owner">The pet owner</param>
        public Pet(int id, Mobile owner) 
            : base(new NpcSpawn() { 
                Id = id,
                SpawnPoint = owner.Position.Copy(),
                MovementArea = null,
                Rotation = -1
            })
        {
            Owner = owner;
            Movement.FollowLeader = owner;
        }

        public override void ResetFlags()
        {
            throw new NotImplementedException();
        }

    }
}
