using RSPS.src.entity.npc;
using RSPS.src.entity.player;
using RSPS.src.net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
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
        /// Creates a new world
        /// </summary>
        /// <param name="id">The world identifier</param>
        /// <param name="connectionListener">The connection listener to use</param>
        public World(int id, ConnectionListener connectionListener) {
            Id = id;
            ConnectionListener = connectionListener;
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
        public async Task Process() {
            if (Online) {
                Console.Error.WriteLine("Unable to process: World {0} is already online", Id);
                return;
            }
            Console.WriteLine("Starting processing world {0}", Id);
            Online = true;

            //TaskManager.StartTaskManager();
            //ReadItemPrices.ReadPrices();

            ParallelOptions mainParallelOptions = new() { MaxDegreeOfParallelism = 25 };

            while (Online) {
                DateTime cycleStart = DateTime.Now;

                if (Players.Entities.Count > 0) {

                }
                try {
                    if (Players.Entities.Count > 0) {
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
                        // Reset the NPC flags
                        Npcs.Entities.ForEach(npc => npc.ResetFlags());
                    }
                    if (ConnectionListener.PendingLogin.Count > 0) {
                        int maxLoginsPerCycle = 100;
                        int loginsHandled = 0;

                        while (ConnectionListener.PendingLogin.Count > 0 && loginsHandled < maxLoginsPerCycle) {
                            Player player = ConnectionListener.PendingLogin.Dequeue();

                            if (player == null) {
                                continue;
                            }
                            player.LoginPlayer();//this goes first
                            player.InitializePlayerSession();
                            Players.Add(player);

                            ConnectionListener.Connections.Add(player.PlayerConnection);
                            
                            loginsHandled++;
                        }
                    }
                }
                catch (Exception ex) {
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

        public void Dispose() {
            GC.SuppressFinalize(this);

            Online = false;

            ConnectionListener.Dispose();

            Npcs.Dispose();
            Players.Dispose();

            Console.WriteLine("Disposed world {0}", Id);
        }

    }
}
