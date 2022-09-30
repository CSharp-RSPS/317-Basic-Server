using RSPS.Entities.Mobiles;
using RSPS.Entities.movement;
using RSPS.Entities.movement.Locations;
using RSPS.Net.GamePackets;
using RSPS.Util.Maths;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Mobiles.Npcs
{
    /// <summary>
    /// Handles movement for npc's
    /// </summary>
    public sealed class NpcMovement : MobileMovement
    {

        /// <summary>
        /// The chance percentage an npc will move within a server cycle.
        /// </summary>
        private static readonly int PassiveMoveChance = 70;

        /// <summary>
        /// The chance that the npc' move is just a sidestep instead of walking around.
        /// </summary>
        private static readonly int SideStepChance = 75;


        /// <summary>
        /// Handles the movement of an NPC in a game tick
        /// </summary>
        /// <param name="npc">The NPC</param>
        public static void HandleMovement(Npc npc)
        {
            if (npc.NpcSpawn.MovementArea == null 
                || npc.LocalPlayers.Count == 0
                || npc.IsDisabled)
            {
                return;
            }
            //TODO: if in combat - return,

            if (RandomUtil.GetPercentage() <= PassiveMoveChance
                && npc.NpcSpawn.MovementArea.InArea(npc.Position))
            {
                return;
            }
            PassiveMove(npc);
        }

        /// <summary>
        /// Handles a passive move of an NPC
        /// </summary>
        /// <param name="npc">The NPC</param>
        public static void PassiveMove(Npc npc)
        {
            if (npc.NpcSpawn.MovementArea == null)
            {
                return;
            }
            Position moveToPos = new(0, 0);

            if (RandomUtil.GetPercentage() <= SideStepChance
                    && npc.NpcSpawn.MovementArea.InArea(npc.Position))
            {
                while (!npc.NpcSpawn.MovementArea.InArea(moveToPos))
                {
                    moveToPos = new Position(
                        npc.Position.X + (RandomUtil.RandomInt(2) - 1),
                        npc.Position.Y + (RandomUtil.RandomInt(2) - 1)
                        );
                }
            }
            else
            {
                moveToPos = npc.NpcSpawn.MovementArea.GetRandomPosition();
            }
            MovementHandler.WalkTo(npc, moveToPos);
        }

    }
}
