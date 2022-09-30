using RSPS.Entities.Mobiles.Players;
using RSPS.Net.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Worlds
{
    /// <summary>
    /// Handles world related operations and data
    /// </summary>
    public static class WorldHandler
    {

        /// <summary>
        /// The identifier for the main development world
        /// </summary>
        public static readonly int DevevelopmentWorldId = 1337;

        /// <summary>
        /// The development world
        /// </summary>
        private static readonly World DevelopmentWorld = new(new("0.0.0.0", 43594), new(DevevelopmentWorldId, "Development World", true));

        /// <summary>
        /// The currently active world
        /// </summary>
        public static World World { get; private set; } = DevelopmentWorld;

        /// <summary>
        /// The identifier for the main test world
        /// </summary>
        public static readonly int TestWorldId = 0;

        /// <summary>
        /// Holds the registered worlds
        /// </summary>
        public static readonly List<World> Worlds = new();


        static WorldHandler()
        {
            Register(DevelopmentWorld);

            Register(new(new("0.0.0.0", 5556), new(TestWorldId, "Test World", true)));
            Register(new(new("0.0.0.0", 5555), new(1, "Wynn's Framework"))); // Live world
        }

        /// <summary>
        /// Validates whether all worlds are valid
        /// </summary>
        /// <returns>The result</returns>
        public static bool ValidateWorlds()
        {
            foreach (World world in Worlds)
            {
                foreach (World other in Worlds.Where(w => w != world))
                {
                    if (world.Details.Id == other.Details.Id)
                    {
                        Console.Error.WriteLine("Multiple worlds using the same identifier: {0}", world.Details.Id);
                        return false;
                    }
                    if (world.Endpoint.Port == other.Endpoint.Port)
                    {
                        Console.Error.WriteLine("Multiple worlds using the same port: {0}", world.Endpoint.Port);
                       //return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Attempts to dispose all worlds
        /// </summary>
        public static void DisposeAllWorlds()
        {
            Worlds.ForEach(w => w.Dispose());
        }

        /// <summary>
        /// Registers a new world
        /// </summary>
        /// <param name="world">The world</param>
        public static void Register(World world)
        {
            Worlds.Add(world);
        }

        /// <summary>
        /// Unregisters a world
        /// </summary>
        /// <param name="world">The world</param>
        public static void Unregister(World world)
        {
            world.Dispose();
            Worlds.Remove(world);
        }

        /// <summary>
        /// Retrieves a world by it's identifier
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <returns>The world</returns>
        public static World? ById(int id)
        {
            return Worlds.FirstOrDefault(w => w.Details.Id == id);
        }

    }
}
