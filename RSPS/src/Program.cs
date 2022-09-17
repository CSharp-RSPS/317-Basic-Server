using RSPS.src.entity;
using RSPS.src.entity.npc;
using RSPS.src.entity.player;
using RSPS.src.net;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using RSPS.src.readfiles;
using RSPS.src.task;
using RSPS.src.Worlds;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Numerics;

namespace RSPS.src
{

    //TODO: Implement player walking
    public class Program
    {

        private static readonly bool Debug = true;


        /// <summary>
        /// The pinpoint of the application
        /// </summary>
        public static void Main()
        {
            if (Debug)
            {
                World? debugWorld = WorldContainer.ById(WorldContainer.DevevelopmentWorldId);

                if (debugWorld == null)
                {
                    Console.Error.WriteLine("Debug world {0} not registered", WorldContainer.DevevelopmentWorldId);
                    return;
                }
                if (!debugWorld.Initialize())
                {
                    Console.Error.WriteLine("Failed to initialize world {0}:{1}", debugWorld.Id, debugWorld.Name);
                    return;
                }
                debugWorld.Start().Wait();
            }
            else
            {
                if (!WorldContainer.ValidateWorlds())
                {
                    Console.Error.WriteLine("Worlds collection invalid");
                    return;
                }
                foreach (World world in WorldContainer.Worlds)
                {
                    if (!world.Initialize())
                    {
                        Console.Error.WriteLine("Failed to initialize world {0}", world.Id);
                        continue;
                    }
                }
                List<System.Threading.Tasks.Task> worldTasks = new();

                foreach (World world in WorldContainer.Worlds.Where(w => w.Initialized))
                {
                    worldTasks.Add(world.Start());
                }
                System.Threading.Tasks.Task.WaitAll(worldTasks.ToArray());
            }
            Console.Error.WriteLine("Application exited, no worlds were being handled anymore.");
        }

    }

}