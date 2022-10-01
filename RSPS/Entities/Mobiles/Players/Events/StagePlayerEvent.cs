using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Mobiles.Players.Events
{
    /// <summary>
    /// Represents a player action event that goes through different stages
    /// </summary>
    public abstract class StagePlayerEvent : PlayerEvent
    {

        /// <summary>
        /// The amount of stages
        /// </summary>
        public int Stages { get; private set; }

        /// <summary>
        /// The current stage
        /// </summary>
        public int Stage { get; protected set; }


        /// <summary>
        /// Creates a new stage player event
        /// </summary>
        /// <param name="multipleAllowed">The player event type</param>
        /// <param name="stages">The amount of stages</param>
        protected StagePlayerEvent(PlayerEventType playerEventType, int stages)
            : base(playerEventType)
        {
            Stages = stages;
        }

        /// <summary>
        /// Executes an event stage
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="stage">The stage</param>
        /// <returns>Whether the stage was successful</returns>
        protected abstract bool ExecuteStage(Player player, int stage);

        public override bool Execute(Player player)
        {
            if (!ExecuteStage(player, Stage))
            {
                Cancel();
                return true;
            }
            Stage++;
            return Stage >= Stages;
        }

        /// <summary>
        /// Cancels the active stage event
        /// </summary>
        protected abstract void Cancel();

    }
}
