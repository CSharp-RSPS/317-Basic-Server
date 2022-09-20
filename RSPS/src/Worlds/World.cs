using RSPS.src.entity.npc;
using RSPS.src.entity.player;
using RSPS.src.net;
using RSPS.src.net.Authentication;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using RSPS.src.schedule;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.Worlds
{
    /// <summary>
    /// Represents a world
    /// </summary>
    public class World : IDisposable
    {

        /// <summary>
        /// The network endpoint
        /// </summary>
        public NetEndpoint Endpoint { get; }

        /// <summary>
        /// The world details
        /// </summary>
        public WorldDetails Details { get; private set; }

        /// <summary>
        /// Whether the world was initialized
        /// </summary>
        public bool Initialized { get; private set; }

        /// <summary>
        /// Whether the world is online
        /// </summary>
        public bool Online { get; set; }

        /// <summary>
        /// Handles connections networking
        /// </summary>
        public readonly ConnectionListener ConnectionListener;

        /// <summary>
        /// Manages NPC's
        /// </summary>
        public readonly NpcManager Npcs;

        /// <summary>
        /// Manages players
        /// </summary>
        public readonly PlayerManager Players;

        /// <summary>
        /// The max. login and/or logout operations allowed per cycle
        /// </summary>
        private readonly int _maxLoginLogoutOpsPerCycle;

        /// <summary>
        /// The default parallel options
        /// </summary>
        private readonly ParallelOptions mainParallelOptions;

        /// <summary>
        /// Holds the cycle times
        /// </summary>
        private readonly Queue<int> cycleTimes;


        /// <summary>
        /// Creates a new world
        /// </summary>
        /// <param name="endpoint">The network endpoint</param>
        /// <param name="details">The world details</param>
        /// <param name="maxLoginLogoutOpsPerCycle">The max. login and/or logout operations allowed per cycle</param>
        public World(NetEndpoint endpoint, WorldDetails details, int maxLoginLogoutOpsPerCycle = 100)
        {
            Endpoint = endpoint;
            Details = details;
            _maxLoginLogoutOpsPerCycle = maxLoginLogoutOpsPerCycle;

            ConnectionListener = new ConnectionListener();
            ConnectionListener.PlayerAuthenticated += OnPlayerAuthenticated;

            Npcs = new();
            Players = new();

            mainParallelOptions = new() { MaxDegreeOfParallelism = 25 };
            cycleTimes = new();
        }

        /// <summary>
        /// Initializes a world to go online
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            if (Online)
            {
                Console.Error.WriteLine("Unable to initialize: World {0} is already online", Details.Id);
                return false;
            }
            if (!ConnectionListener.Start(Endpoint))
            {
                return false;
            }
            Initialized = true;
            Console.WriteLine("{0}: World {1} has been initialized on {2}", Details.Name, Details.Id, Endpoint);
            return true;
        }

        /// <summary>
        /// Processes the world
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            if (!Initialized)
            {
                Console.Error.WriteLine("Unable to start world {0} as it has not been initialized yet", Details.Id);
                return;
            }
            if (Online)
            {
                Console.Error.WriteLine("Unable start world {0} as it's already online", Details.Id);
                return;
            }
            Console.WriteLine("Starting world {0}", Details.Id);

            Scheduler.Start();
            //TaskManager.StartTaskManager();
            //ReadItemPrices.ReadPrices();

            await Process();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

            Initialized = false;
            Online = false;

            ConnectionListener.Dispose();

            Npcs.Dispose();
            Players.Dispose();

            Console.WriteLine("Disposed world {0}", Details.Id);
        }

        /// <summary>
        /// Handles a newly authenticated player
        /// </summary>
        /// <param name="player">The player</param>
        private void OnPlayerAuthenticated(Player player)
        {
            PlayerManager.InitializeSession(player);
            PlayerManager.Login(player, Details);
            Players.PendingLogin.Enqueue(player);
        }

        /// <summary>
        /// Processes the world
        /// </summary>
        /// <returns></returns>
        private async Task Process()
        {
            Online = true;

            while (Online)
            {
                DateTime cycleStart = DateTime.Now;

                try
                {
                    if (Players.Entities.Count > 0)
                    {
                        Console.WriteLine();
                    }
                    // Find any disconnected players that aren't inside the disconnected list yet
                    Players.Entities.Where(p => !Players.Disconnected.Contains(p) 
                        && p.PlayerConnection.ConnectionState == ConnectionState.Disconnected && p.LoggedIn)
                        .ToList().ForEach(p => Players.Disconnected.Add(p));

                    // Add any logged-out players who are pending removal
                    Players.Entities.Where(p => !Players.PendingRemoval.Contains(p)
                        && p.PlayerConnection.ConnectionState == ConnectionState.Authenticated && !p.LoggedIn)
                        .ToList().ForEach(p => Players.PendingRemoval.Enqueue(p));

                    // Attempt to logout any players that are disconnected
                    for (int i = Players.Disconnected.Count - 1; i >= 0; i--)
                    {
                        Player? player = Players.Disconnected[i];

                        if (player == null)
                        {
                            Players.Disconnected.RemoveAt(i);
                            continue;
                        }
                        if (!PlayerManager.Logout(player))
                        {
                            continue;
                        }
                        Players.Disconnected.RemoveAt(i);
                        Players.PendingRemoval.Enqueue(player);
                    }
                    if (Players.PendingLogin.Count > 0 || Players.PendingRemoval.Count > 0)
                    { // Remove logged out players and add new logins from/to the world
                        int removalOps = Players.PendingRemoval.Count;
                        int loginOps = Players.PendingLogin.Count;

                        if ((removalOps + loginOps) > _maxLoginLogoutOpsPerCycle)
                        { // When the total operations exceed the max. allowed, balance out between logins and removals
                            int balancedOps = (_maxLoginLogoutOpsPerCycle / 2);

                            removalOps = loginOps < balancedOps ? (_maxLoginLogoutOpsPerCycle - loginOps) : balancedOps;
                            loginOps = removalOps < balancedOps ? (_maxLoginLogoutOpsPerCycle - removalOps) : balancedOps;
                        }
                        Parallel.For(0, removalOps, mainParallelOptions, _ =>
                        {
                            if (!Players.PendingRemoval.TryDequeue(out Player? player) || player == null)
                            {
                                return;
                            }
                            player.PlayerConnection.Dispose();
                            Players.Remove(player);
                        });
                        Parallel.For(0, loginOps, mainParallelOptions, _ =>
                        {
                            if (!Players.PendingLogin.TryDequeue(out Player? player) || player == null)
                            {
                                return;
                            }
                            Players.Add(player);
                        });
                    }
                    if (Players.Entities.Count > 0)
                    { // Handle active players in the world
                        if (Players.Entities.Contains(null))
                        {
                            Console.Error.WriteLine("Nulled player slipped into players list somehow in world {0}", Details.Id);
                            return;
                        }
                        // Process the movement of players
                        Parallel.ForEach(Players.Entities, mainParallelOptions, (Player? player) => player?.MovementHandler.ProcessMovements());

                        // Process player updating
                        Players.Entities.ForEach(p => {
                            PlayerUpdating.Update(this, p);
                            //NpcUpdating.Update(player);
                        });/*
                        Parallel.ForEach(Players.Entities, mainParallelOptions, (Player? player) =>
                        {
                            if (player != null)
                            {
                                PlayerUpdating.Update(this, player);
                            }
                            //NpcUpdating.Update(player);
                            
                        });*/
                        // Reset the player flags
                        Parallel.ForEach(Players.Entities, mainParallelOptions, (Player? player) => player?.ResetFlags());

                        Console.WriteLine("Processed {0} active players in world {1}", Players.Entities.Count, Details.Id);
                    }
                    if (Npcs.Entities.Count > 0)
                    { // Handle NPC's
                        // Reset the NPC flags
                        Parallel.ForEach(Npcs.Entities, mainParallelOptions, (Npc? npc) => npc?.ResetFlags());

                        Console.WriteLine("Processed {0} npc's in world {1}", Npcs.Entities.Count, Details.Id);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    break;
                }
                TimeSpan cycleElapsed = DateTime.Now.Subtract(cycleStart);
                CycleInfo(cycleElapsed);
                int sleepTime = Constants.WorldCycleMs - cycleElapsed.Milliseconds;
                await Task.Delay(sleepTime < 0 ? 0 : sleepTime);
            }
            Console.Error.WriteLine("Stopped processing world {0}, disposing...", Details.Id);
            Dispose();
        }

        /// <summary>
        /// Handles cycle information
        /// </summary>
        /// <param name="cycleTime">The cycle time</param>
        private void CycleInfo(TimeSpan cycleTime)
        {
            if (cycleTimes.Count > 100)
            {
                cycleTimes.Dequeue();
            }
            cycleTimes.Enqueue(cycleTime.Milliseconds);

            double average = Math.Round(cycleTimes.Average(), 2);

            Console.WriteLine("Processing world {0} took {1}ms (Average: {2}ms)", Details.Id, cycleTime.Milliseconds, average);
        }

    }
}
