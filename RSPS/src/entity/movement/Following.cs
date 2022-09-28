using RSPS.src.entity.Mobiles;
using RSPS.src.entity.Mobiles.Npcs.Pets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.movement
{
    /// <summary>
    /// Handles following related operations
    /// </summary>
    public static class Following
    {


        /// <summary>
        /// Makes a mobile start following another mobile
        /// </summary>
        /// <param name="follower">the following mobile</param>
        /// <param name="leader">The leader the mobile will follow</param>
        /// <returns>Whether starting following was successful</returns>
        public static bool StartFollowing(Mobile follower, Mobile leader)
        {
            if (!follower.Position.IsWithinDistance(leader.Position, 40))
            {
                return false;
            }
            MovementHandler.PrepareMovement(follower);
            follower.Movement.FollowLeader = leader;
            return Follow(follower);
        }

        /// <summary>
        /// Handles the following of a follower
        /// </summary>
        /// <param name="follower">The follower</param>
        /// <returns>Whether following was succesful</returns>
        public static bool Follow(Mobile follower)
        {
            Mobile? leader = follower.Movement.FollowLeader;

            if (leader == null || !follower.Position.IsWithinDistance(leader.Position, 40))
            {
                if (follower is Pet pet)
                {
                    if (leader == null)
                    {
                        //TODO remove npc from the world
                        return false;
                    }
                    pet.Position.SetNewPosition(leader.Position);
                    return true;
                }
                follower.Movement.FollowLeader = null;
                //TODO: Add focus state update (null)
                return false;
            }
            if (follower.Position.Equals(leader.Position))
            {
                MovementHandler.StepAway(follower);
                //TODO: Add focus state update for follower to (leader)
                return true;
            }
            if (follower.Position.IsWithinDistance(leader.Position, 1))
            {
                return true;
            }
            MovementHandler.WalkNearby(follower, leader.Position.Copy());
            return true;
        }

    }
}
