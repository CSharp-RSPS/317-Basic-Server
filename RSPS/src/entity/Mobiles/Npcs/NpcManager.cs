using RSPS.src.entity.Mobiles.Npcs.Definitions;
using RSPS.src.entity.movement.Locations.Regions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.Mobiles.Npcs
{
    /// <summary>
    /// Manages NPC's and handles NPC related operations
    /// </summary>
    public sealed class NpcManager : MobileManager<Npc>
    {

        /// <summary>
        /// The time in milliseconds required for a npc to despawn when no players area nearby.
        /// </summary>
        public static readonly int NpcDespawnTime = 5 * 60 * 1000;

        /// <summary>
        /// Holds the NPC definitions
        /// </summary>
        public static readonly Dictionary<int, NpcDefinition> Defs = new();

        /// <summary>
        /// Holds the NPC spawn definitions
        /// </summary>
        public static readonly List<NpcSpawn> Spawns = new();


        static NpcManager()
        {

        }

        public override void PrepareTick(Npc npc)
        {
            // TODO: yelling

            if (npc.Rotation == -1)
            {
                // TODO: update face state
            }
            if (npc.Movement.FollowLeader == null)
            {
                NpcMovement.HandleMovement(npc);
            }
            base.PrepareTick(npc);
        }

        public override void FinishTick(Npc npc)
        {
            base.FinishTick(npc);
        }

        /// <summary>
        /// Loads the NPC's in a region
        /// </summary>
        /// <param name="region">The region</param>
        public void LoadRegionalNpcs(Region region)
        {
            foreach (NpcSpawn spawn in Spawns.Where(s => !s.Spawned))
            {
                int regionId = RegionManager.GetRegionId(spawn.SpawnPoint);

                if (region.Id != regionId)
                {
                    continue;
                }
                spawn.Spawned = true;
                SpawnNpc(spawn);
            }
        }

        /// <summary>
        /// Creates a new permanent NPC dynamically
        /// </summary>
        /// <param name="spawn">The spawn definition</param>
        /// <returns>The NPC</returns>
        public Npc? CreateNpc(NpcSpawn spawn)
        {
            Npc? npc = SpawnNpc(spawn);
            Spawns.Add(spawn);
            //TODO: Save spawns to json file
            return npc;
        }

        /// <summary>
        /// Spawns an NPC into the world
        /// </summary>
        /// <param name="spawn">The spawn definition</param>
        /// <returns>The NPC</returns>
        public Npc? SpawnNpc(NpcSpawn spawn)
        {
            NpcDefinition? def = GetDef(spawn.Id);

            if (def == null)
            {
                return null;
            }
            //TODO: Check for comabtable npc def
            Npc npc = new(spawn);
            Add(npc);
            return npc;
        }

        /// <summary>
        /// Retrieves the definition for an NPC with a given identifier
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>The definition</returns>
        public static NpcDefinition? GetDef(int id)
        {
            return Defs.ContainsKey(id) ? Defs[id] : null;
        }

        public override Npc Add(Npc npc)
        {
            base.Add(npc);

            npc.WorldIndex = Entities.IndexOf(npc);
            return npc;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);

            //TODO any logic

            base.Dispose();
        }

    }
}
