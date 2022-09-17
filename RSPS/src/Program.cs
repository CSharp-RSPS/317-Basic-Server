using RSPS.src.entity;
using RSPS.src.entity.npc;
using RSPS.src.entity.player;
using RSPS.src.net;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using RSPS.src.readfiles;
using RSPS.src.task;
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

        public static readonly World DevWorld = new(1337, new ConnectionListener("0.0.0.0", 43594)); //Development world


        /// <summary>
        /// The pinpoint of the application
        /// </summary>
        public static void Main()
        {
            // Initialize the world
            if (!DevWorld.Initialize()) {
                return;
            }
            // Start processing the world
            //System.Threading.Tasks.Task.WaitAll(new System.Threading.Tasks.Task[] { world.Process() }); //when multiple worlds
            DevWorld.Start().Wait();
        }

    }

}