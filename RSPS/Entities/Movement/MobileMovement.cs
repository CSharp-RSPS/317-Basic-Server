using RSPS.Entities.Mobiles;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.movement
{
    /// <summary>
    /// Handles the movement of a mobile entity
    /// </summary>
    public abstract class MobileMovement
    {

        /// <summary>
        /// The current walking direction
        /// </summary>
        public DirectionType WalkingDirection { get; set; } = DirectionType.None;

        /// <summary>
        /// The current running direction
        /// </summary>
        public DirectionType RunningDirection { get; set; } = DirectionType.None;

        /// <summary>
        /// The leader in case we're following another mobile
        /// </summary>
        public Mobile? FollowLeader { get; set; } = null;

        /// <summary>
        /// Whether we're following another mobile
        /// </summary>
        public bool HasFollowLeader => FollowLeader != null;

        /// <summary>
        /// Whether we just teleported
        /// </summary>
        public bool Teleported { get; set; } = true;

        /// <summary>
        /// The movement focus points queue
        /// </summary>
        public Queue<MovementPoint> MovementPoints { get; private set; } = new();


        /// <summary>
        /// Updates the movement
        /// </summary>
        /// <param name="mob">The mobile we're updating the movement for</param>
        /// <param name="writer">The packet writer of the player requesting updates</param>
        public void Update(Mobile mob, PacketWriter writer)
        {
            if (mob.Movement.WalkingDirection == DirectionType.None)
            { // Standing still
                if (mob.UpdateRequired)
                {
                    writer.WriteBits(1, 1);
                    writer.WriteBits(2, 0);
                    return;
                }
                writer.WriteBits(1, 0);
                return;
            }
            // Moving (walking by default)
            writer.WriteBits(1, 1);
            writer.WriteBits(2, 1);
            writer.WriteBits(3, mob.Movement.WalkingDirection.GetDirectionValue());

            if (mob.Movement.RunningDirection != DirectionType.None)
            { // Running, should only happen for players
                writer.WriteBits(3, mob.Movement.RunningDirection.GetDirectionValue());
            }
            writer.WriteBits(1, mob.UpdateRequired ? 1 : 0);
        }

        /// <summary>
        /// Retrieves whether a mobile can move
        /// </summary>
        /// <param name="mob">The mobile</param>
        /// <returns>The result</returns>
        public virtual bool CanMove(Mobile mob)
        {
            return !mob.IsDisabled;
        }

        /// <summary>
        /// Indicates movement handling has finished
        /// </summary>
        public void FinishMovement()
        {
            if (MovementPoints.Count > 0)
            {
                MovementPoints.Dequeue();
            }
        }

    }
}
