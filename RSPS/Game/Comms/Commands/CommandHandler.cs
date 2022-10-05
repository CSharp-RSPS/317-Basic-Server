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
using System.Numerics;

namespace RSPS.Game.Comms.Commands
{
    /// <summary>
    /// Handles commands
    /// </summary>
    public static class CommandHandler
    {

        /// <summary>
        /// Holds the available commands
        /// </summary>
        private static readonly List<ICommand> Commands = new();

        static CommandHandler()
        {
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "interface" }, new int[] { 1 }) {
                Execute = (player, args) => { 
                    if (!int.TryParse(args[0], out int interfaceId))
                    {
                        PacketHandler.SendPacket(player, new SendMessage("Invalid interface ID"));
                        return;
                    }
                    Interfaces.OpenInterface(player, interfaceId);
                }
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "bank" })
            {
                Execute = (player, args) => BankingHandler.OpenBank(player)
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "item" }, new int[] { 1, 2 })
            {
                Execute = (player, args) => {
                    int amount = 1;

                    if (!int.TryParse(args[0], out int itemId))
                    {
                        PacketHandler.SendPacket(player, new SendMessage("Invalid item ID"));
                        return;
                    }
                    if (args.Length > 1 && !int.TryParse(args[1], out amount))
                    {
                        PacketHandler.SendPacket(player, new SendMessage("Invalid item amount"));
                        return;
                    }
                    player.Inventory.AddItem(itemId, amount);
                    ItemManager.RefreshInterfaceItems(player, player.Inventory.Items, Interfaces.GetInventoryRefreshInterface(player));
                }
            });
            Commands.Add(new Command(PlayerRights.Moderator, new string[] { "animreset", "resetanim", "resetanims", "resetanimation", "resetanimations" })
            {
                Execute = (player, args) => PacketHandler.SendPacket(player, PacketDefinition.AnimationReset)
            });
        }

        /// <summary>
        /// Handles a command for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="command">The command</param>
        public static void HandleCommand(Player player, string command)
        {
            string[] parts = command.Split(" ");

            if (parts.Length <= 0)
            {
                return;
            }
            string identifier = parts[0];
            string[] arguments = Array.Empty<string>();

            if (parts.Length > 1)
            { // Store the command arguments into an array
                arguments = new string[parts.Length - 1];

                for (int i = 1; i < parts.Length; i++)
                {
                    arguments[i - 1] = parts[i];
                }
            }
            ICommand? cmd = Commands.FirstOrDefault(command => command.MatchesIdentifier(identifier));

            if (cmd == null || !cmd.HasRequiredRights(player.PersistentVars.Rights))
            {
                return;
            }
            if (!cmd.HasRequiredArguments(arguments))
            {
                PacketHandler.SendPacket(player, new SendMessage("Malformed command"));
                return;
            }
            cmd.Execute(player, arguments);
        }

    }
}
