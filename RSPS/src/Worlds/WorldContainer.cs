using RSPS.src.net.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.Worlds
{
    public static class WorldContainer
    {

        /// <summary>
        /// The identifier for the main development world
        /// </summary>
        public static readonly int DevevelopmentWorldId = 1337;

        /// <summary>
        /// The identifier for the main test world
        /// </summary>
        public static readonly int TestWorldId = 1338;

        /// <summary>
        /// Holds the registered worlds
        /// </summary>
        public static readonly List<World> Worlds = new();


        static WorldContainer()
        {
            Register(new(DevevelopmentWorldId, "Development World", new ConnectionListener("0.0.0.0", 43594))
            { // Dev world
                Debugging = true
            });
            Register(new(TestWorldId, "Test World", new ConnectionListener("0.0.0.0", 5556))
            { // Test world
                Debugging = true
            });
            Register(new(1, "Wynn's Framework", new ConnectionListener("0.0.0.0", 5555))); // Live world
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
                    if (world.Id == other.Id)
                    {
                        Console.Error.WriteLine("Multiple worlds using the same identifier: {0}", world.Id);
                        return false;
                    }
                    if (world.ConnectionListener.Port == other.ConnectionListener.Port)
                    {
                        Console.Error.WriteLine("Multiple worlds using the same port: {0}", world.ConnectionListener.Port);
                        return false;
                    }
                }
            }
            return true;
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
            return Worlds.FirstOrDefault(w => w.Id == id);
        }

    }
}
