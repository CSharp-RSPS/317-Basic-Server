using RSPS.Entities;
using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Items.Equipment;
using RSPS.Net;
using RSPS.Net.Connections;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Worlds;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace RSPS
{
    public class Program
    {


        /// <summary>
        /// The pinpoint of the application
        /// </summary>
        public static void Main()
        {
            EquipmentHandler.Temp();

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