using RSPS.src.entity.npc;
using RSPS.src.entity.player;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src
{
    /// <summary>
    /// Represents a world
    /// </summary>
    public class World : IDisposable
    {

        /// <summary>
        /// The world identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Whether the world is online
        /// </summary>
        public bool Online { get; set; }

        /// <summary>
        /// Handles connections networking
        /// </summary>
        public ConnectionListener ConnectionListener { get; private set; }

        /// <summary>
        /// Manages NPC's
        /// </summary>
        public readonly NpcManager Npcs = new();

        /// <summary>
        /// Manages players
        /// </summary>
        public readonly PlayerManager Players = new();

        /// <summary>
        /// The max. logins allowed per cycle
        /// </summary>
        private readonly int _maxLoginsPerCycle;


        /// <summary>
        /// Creates a new world
        /// </summary>
        /// <param name="id">The world identifier</param>
        /// <param name="connectionListener">The connection listener to use</param>
        public World(int id, ConnectionListener connectionListener, int maxLoginsPerCycle = 100) {
            Id = id;
            ConnectionListener = connectionListener;
            _maxLoginsPerCycle = maxLoginsPerCycle;
        }

        /// <summary>
        /// Initializes a world to go online
        /// </summary>
        /// <returns></returns>
        public bool Initialize() {
            if (Online) {
                Console.Error.WriteLine("Unable to initialize: World {0} is already online", Id);
                return false;
            }
            if (!ConnectionListener.Start()) {
                return false;
            }
            Console.WriteLine("{0}: World {1} has been initialized on {2}:{3}", Constants.SERVER_NAME, Id, 
                ConnectionListener.Endpoint, ConnectionListener.Port);
            return true;
        }

        /// <summary>
        /// Sets a new connection listener for the world
        /// </summary>
        /// <param name="connectionListener"></param>
        public void SetConnectionListener(ConnectionListener connectionListener) {
            ConnectionListener.Dispose();
            ConnectionListener = connectionListener;
        }

        /// <summary>
        /// Processes the world
        /// </summary>
        /// <returns></returns>
        public async Task Start() {
            if (Online) {
                Console.Error.WriteLine("Unable to process: World {0} is already online", Id);
                return;
            }
            Console.WriteLine("Starting processing world {0}", Id);
            Online = true;

            //TaskManager.StartTaskManager();
            //ReadItemPrices.ReadPrices();

            await Process();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            Online = false;

            ConnectionListener.Dispose();

            Npcs.Dispose();
            Players.Dispose();

            Console.WriteLine("Disposed world {0}", Id);
        }

        /// <summary>
        /// Processes the world
        /// </summary>
        /// <returns></returns>
        private async Task Process()
        {
            ParallelOptions mainParallelOptions = new() { MaxDegreeOfParallelism = 25 };

            while (Online)
            {
                DateTime cycleStart = DateTime.Now;

                try
                {
                    // Handle disconnected players
                    Parallel.ForEach(Players.Entities.Where(p => !p.PlayerConnection.IsConnected), mainParallelOptions, player => {
                        Console.WriteLine("Handling removal of disconnected player {0}", player.Credentials.Username);
                        //TODO if in a fight, continue etc...
                        Players.Logout(player, false);
                    });
                    if (Players.Entities.Count > 0)
                    { // Handle active players
                        // Process the movement of players
                        Parallel.ForEach(Players.Entities, mainParallelOptions, player => player.MovementHandler.ProcessMovements());
                        // Process player updating
                        Parallel.ForEach(Players.Entities, mainParallelOptions, player =>
                        {
                            //NpcUpdating.Update(player);
                            PlayerUpdating.Update(this, player);
                        });
                        // Reset the player flags
                        Parallel.ForEach(Players.Entities, mainParallelOptions, player => player.ResetFlags());
                        
                        Console.WriteLine("Processed {0} active players", Players.Entities.Count);
                    }
                    if (Npcs.Entities.Count > 0)
                    { // Handle NPC's
                        // Reset the NPC flags
                        Parallel.ForEach(Npcs.Entities, mainParallelOptions, npc => npc.ResetFlags());

                        Console.WriteLine("Processed {0} npc's", Npcs.Entities.Count);
                    }
                    if (ConnectionListener.PendingLogin.Count > 0)
                    { // Handle any pending player logins
                        int loginsHandled = 0;

                        while (ConnectionListener.PendingLogin.Count > 0 && loginsHandled < _maxLoginsPerCycle)
                        {
                            Player player = ConnectionListener.PendingLogin.Dequeue();

                            if (player == null)
                            {
                                continue;
                            }
                            player.InitializePlayerSession();

                            Players.Add(player);
                            ConnectionListener.Connections.Add(player.PlayerConnection);

                            loginsHandled++;
                        }
                        Console.WriteLine("Handled {0} new player logins", loginsHandled);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    break;
                }
                TimeSpan cycleElapsed = DateTime.Now.Subtract(cycleStart);
                Console.WriteLine("Processing world {0} took {1}ms", Id, cycleElapsed.Milliseconds);
                int sleepTime = Constants.WorldCycleMs - cycleElapsed.Milliseconds;
                await Task.Delay(sleepTime < 0 ? 0 : sleepTime);
            }
            Console.Error.WriteLine("Stopped processing world {0}, disposing...", Id);
            Dispose();
        }

    }
}
