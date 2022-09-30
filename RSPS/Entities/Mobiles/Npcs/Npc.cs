using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.Entities.Mobiles;
using RSPS.Entities.Mobiles.Npcs.Definitions;
using RSPS.Entities.movement.Locations;

namespace RSPS.Entities.Mobiles.Npcs
{
    /// <summary>
    /// Represents a non-playable character
    /// </summary>
    public class Npc : Mobile
    {

        /// <summary>
        /// The NPC spawn definition
        /// </summary>
        public NpcSpawn NpcSpawn { get; private set; }

        /// <summary>
        /// The NPC identifier
        /// </summary>
        public int Id => NpcSpawn.Id;

        /// <summary>
        /// The min. distance to be able to interact with the NPC
        /// </summary>
        public int InteractionDistance { get; private set; }

        /// <summary>
        /// The rotation of the NPC
        /// </summary>
        public int Rotation { get; set; }


        /// <summary>
        /// Creates a new NPC
        /// </summary>
        /// <param name="npcSpawn">The NPC spawn definition</param>
        public Npc(NpcSpawn npcSpawn)
            : base(npcSpawn.SpawnPoint, new NpcMovement())
        {
            NpcSpawn = npcSpawn;
            InteractionDistance = 0; //TODO: Get from npc defintiions (size)
            Rotation = NpcSpawn.Rotation;
        }

        /// <summary>
        /// Retrieves the facing position of the NPC
        /// </summary>
        /// <returns>The facing position</returns>
        public Position GetFace()
        {
            int x = Position.X;
            int y = Position.Y;

            if (Rotation == 1)
            {
                x++;
            }
            else if (Rotation == 3)
            {
                x--;
            }
            else if (Rotation == 0)
            {
                y++;
            }
            else if (Rotation == 2)
            {
                y--;
            }
            return new Position(x, y, Position.Z);
        }

        public bool UpdateRequired { get; set; }
        public bool AppearanceUpdateRequired { get; set; }
        public bool ChatUpdateRequired { get; set; }
        public int PrimaryDirection { get; set; }
        public int SecondaryDirection { get; set; }

        public override void ResetFlags()
        {
            UpdateRequired = false;
            AppearanceUpdateRequired = false;
            ChatUpdateRequired = false;
            ResetMovementQueue = false;
            NeedsPlacement = false;
            PrimaryDirection = -1;
            SecondaryDirection = -1;
        }

    }
}
