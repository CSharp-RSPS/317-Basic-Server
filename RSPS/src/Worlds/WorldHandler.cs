using RSPS.src.entity.player;
using RSPS.src.net.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.Worlds
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

        /// <summary>
        /// Resolve the world a player is logged into
        /// </summary>
        /// <param name="player">The player</param>
        /// <returns>The world</returns>
        [Obsolete("Should not be used anymore since only 1 static world can be online", true)]
        public static World? ResolveWorld(Player player)
        {
            return ResolveWorld(player.PlayerConnection);
        }

        /// <summary>
        /// Resolve the world a connection is connected to
        /// </summary>
        /// <param name="connection">The connection</param>
        /// <returns>The world</returns>
        [Obsolete("Should not be used anymore since only 1 static world can be online", true)]
        public static World? ResolveWorld(Connection connection)
        {
            return ById(connection.WorldDetails.Id);
        }

        /// <summary>
        /// Finds a player by their username within all worlds
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>The result</returns>
        [Obsolete("Use the method within the playermanager", true)]
        public static Player? FindPlayerByUsername(string username)
        {
            return World?.Players.ByUsername(username);
           /* foreach (World w in Worlds)
            {
                Player? player = w.Players.ByUsername(username);

                if (player != null)
                {
                    return player;
                }
            }
            return default;*/
        }

        /// <summary>
        /// Attempts to make a player swap worlds
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="world">The world to swap to</param>
        /// <returns>The result</returns>
        [Obsolete("Should not be used anymore since only 1 static world can be online", true)]
        public static bool SwapWorlds(Player player, World world)
        {
            if (!world.Online)
            {
                return false;
            }
            if (!PlayerManager.Logout(player))
            {
                return false;
            }
            //world.OnPlayerAuthenticated(player);
            //world.ConnectionListener.StartListenForPackets(player);
            return true;
        }

    }
}
