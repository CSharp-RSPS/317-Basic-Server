using RSPS.src.entity;
using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using RSPS.src.readfiles;
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


        /// <summary>
        /// The pinpoint of the application
        /// </summary>
        public static void Main()
        {
            if (!WorldHandler.ValidateWorlds())
            {
                Console.Error.WriteLine("Worlds collection invalid");
                return;
            }
            if (WorldHandler.World == null)
            {
                Console.Error.WriteLine("No world set to run");
                return;
            }
            if (!WorldHandler.World.Initialize())
            {
                Console.Error.WriteLine("Failed to initialize world {0}:{1}", WorldHandler.World.Details.Id, WorldHandler.World.Details.Name);
                return;
            }
            WorldHandler.World.Start().Wait();
        }

    }

}