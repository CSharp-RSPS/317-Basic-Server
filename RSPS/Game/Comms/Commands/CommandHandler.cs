using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets.Send;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Game.Items;
using RSPS.Game.UI;
using RSPS.Game.Banking;

namespace RSPS.Game.Comms.Commands
{
    /// <summary>
    /// Handles commands
    /// </summary>
    public static class CommandHandler
    {


        /// <summary>
        /// Handles a command for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="command">The command</param>
        /// <param name="arguments">The command arguments</param>
        public static void HandleCommand(Player player, string command, string[] arguments)
        {
            if (string.IsNullOrEmpty(command))
            {
                return;
            }
            switch (command)
            {
                case "interface":
                    Interfaces.OpenInterface(player, int.Parse(arguments[1]));
                    break;

                case "bank":
                    BankingHandler.OpenBank(player);
                    break;

                case "item":
                    player.Inventory.AddItem(new Item(int.Parse(arguments[1]), 1));
                    ItemManager.RefreshInterfaceItems(player, player.Inventory.Items, Interfaces.Inventory);
                    //TODO refresh the right interface, could be banking as example so inventory interface is different when banking
                    break;

                case "resetanim":
                case "resetanimation":
                case "resetanimations":
                case "animreset":
                    PacketHandler.SendPacket(player, PacketDefinition.AnimationReset);
                    break;
            }
            PacketHandler.SendPacket(player, new SendMessage(string.Format("Handled command {0}", command)));
        }

    }
}
