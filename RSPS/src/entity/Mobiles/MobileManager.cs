using RSPS.src.entity.Health;
using RSPS.src.entity.Mobiles.Players;
using RSPS.src.entity.movement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.Mobiles
{
    /// <summary>
    /// Manages mobile related operations
    /// </summary>
    /// <typeparam name="T">The type of mobile</typeparam>
    public abstract class MobileManager<T> : EntityManager<T> where T : Mobile
    {


        public override void PrepareTick(T mob)
        {
            if (mob is IHittable)
            {
                // TODO, seperate processor for hittables (can also be objects). when hit add to processor, when health recovered remove from processor
            }
            if (mob.Movement.FollowLeader != null)
            {
                Following.Follow(mob.Movement.FollowLeader);
            }
            MovementHandler.ProcessMovement(mob);
        }

        public override void FinishTick(T mob)
        {
            if (mob.Disabled != null)
            {
                mob.Disabled.TimeDisabled--;

                if (mob.Disabled.TimeDisabled <= 0)
                {
                    mob.Disabled = null;
                }
            }
            mob.Movement.Teleported = false;
            mob.Movement.WalkingDirection = DirectionType.None;
            mob.Movement.RunningDirection = DirectionType.None;
            //TODO clear state updates
        }

        public override T Add(T entity)
        {
            base.Add(entity);

            entity.WorldIndex = GetIndex(entity);

            return entity;
        }

        /// <summary>
        /// Retrieves a mobile by it's world index
        /// </summary>
        /// <param name="worldIndex">The world index</param>
        /// <returns>The player</returns>
        public T? ByWorldIndex(int worldIndex)
        {
            return Entities.FirstOrDefault(e => e.WorldIndex == worldIndex);
        }

    }
}
