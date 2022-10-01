using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Mobiles.Players.Events
{
    /// <summary>
    /// Handles events
    /// </summary>
    public sealed class PlayerEventController
    {

        /// <summary>
        /// The currently active stage player events
        /// </summary>
        public Dictionary<PlayerEventType, StagePlayerEvent> StagePlayerEvents = new();


        /// <summary>
        /// Attempts to start a new player event
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="playerEvent">The player event</param>
        public void Start(Player player, PlayerEvent playerEvent) 
        {
            if (StagePlayerEvents.ContainsKey(playerEvent.PlayerEventType))
            {
                Debug.WriteLine("Player event of type " + playerEvent.PlayerEventType.ToString()
                    + " can not be started as one of the same type is already active");
                return;
            }
            if (playerEvent.Execute(player))
            {
                return;
            }
            if (playerEvent is StagePlayerEvent spe)
            {
                StagePlayerEvents.Add(playerEvent.PlayerEventType, spe);
                return;
            }
            Debug.WriteLine("Player event of type " + playerEvent.PlayerEventType.ToString() + " encountered that is not staged but did not finish");
        }

        /// <summary>
        /// Moves a stage player event to the next stage
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="playerEventType">The player event type</param>
        public void NextStage(Player player, PlayerEventType playerEventType)
        {
            StagePlayerEvent? spe = FindActive<StagePlayerEvent>(playerEventType);

            if (spe == null)
            {
                Debug.WriteLine("No active player stage event found of type " + playerEventType.ToString());
                return;
            }
            NextStage(player, spe);
        }

        /// <summary>
        /// Moves a stage of a given player event to the next stage
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="spe">The stage player event</param>
        public void NextStage(Player player, StagePlayerEvent spe)
        {
            if (spe.Execute(player))
            {
                StagePlayerEvents.Remove(spe.PlayerEventType);
            }
        }

        /// <summary>
        /// Finds an active player event
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="playerEventType">The player event type</param>
        /// <returns>The result</returns>
        public T? FindActive<T>(PlayerEventType playerEventType) where T : StagePlayerEvent
        {
            return StagePlayerEvents.ContainsKey(playerEventType) ? (T)StagePlayerEvents[playerEventType] : null;
        }

    }
}
