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
using RSPS.Entities.movement;
using RSPS.Worlds;

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
        private static readonly List<Command> Commands = new();

        static CommandHandler()
        {
            AddAdministratorCommands();
            AddModeratorCommands();
            AddDefaultCommands();
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

        /// <summary>
        /// Adds the commands only meant for administrators
        /// </summary>
        private static void AddAdministratorCommands()
        {
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "interface" }, new int[] { 1 })
            {
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
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "item", "pickup" }, new int[] { 1, 2 })
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
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "mute" }, new int[] { 1 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "ipmute" }, new int[] { 1 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "unmute" }, new int[] { 1 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "ban" }, new int[] { 1 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "ipban" }, new int[] { 1 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "unban" }, new int[] { 1 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "skill", "setskill" }, new int[] { 2 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "showbank" }, new int[] { 1 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "showinv", "showinventory" }, new int[] { 1 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "copyinv", "copyinventory" }, new int[] { 1 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "copy" }, new int[] { 1 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "give", "giveitem", "giveitems" }, new int[] { 2, 3 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "tp", "tele", "teleport" }, new int[] { 2, 3 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "teleother", "tpother", "teleportother" }, new int[] { 3, 4 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "pnpc" })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "unpnpc" })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "step" })
            {
                Execute = (player, args) => MovementHandler.StepAway(player)
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "spec" })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "config" }, new int[] { 2 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "update" }, new int[] { 1 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "npc" }, new int[] { 2 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "addnpc" }, new int[] { 2 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "rmnpc", "removenpc", "delnpc", "deletenpc" }, new int[] { 2 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "obj", "object" }, new int[] { 2 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "addobj", "addobject" }, new int[] { 2 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "rmobj", "removeobj", "delobj", "deleteobject" }, new int[] { 2 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "gfx", "graphics" }, new int[] { 2 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Administrator, new string[] { "move", "walkto" }, new int[] { 2 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
        }

        /// <summary>
        /// Adds the commands meant for moderators and up
        /// </summary>
        private static void AddModeratorCommands()
        {
            Commands.Add(new Command(PlayerRights.Moderator, new string[] { "animreset", "resetanim", "resetanims", "resetanimation", "resetanimations" })
            {
                Execute = (player, args) => PacketHandler.SendPacket(player, PacketDefinition.AnimationReset)
            });
            Commands.Add(new Command(PlayerRights.Moderator, new string[] { "pos", "position", "mypos", "location" })
            {
                Execute = (player, args) => PacketHandler.SendPacket(player, new SendMessage(player.Position.ToString()))
            });
            Commands.Add(new Command(PlayerRights.Moderator, new string[] { "msg", "message" }, new int[] { 2 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
            Commands.Add(new Command(PlayerRights.Moderator, new string[] { "kick" }, new int[] { 1 })
            {
                Execute = (player, args) => throw new NotImplementedException()
            });
        }

        /// <summary>
        /// Adds the default commands available to everyone
        /// </summary>
        private static void AddDefaultCommands()
        {
            Commands.Add(new Command(PlayerRights.Default, new string[] { "test" }, new int[] { 1 })
            {
                Execute = (player, args) => PacketHandler.SendPacket(player, new SendDisplayHintIcon(int.Parse(args[0])))
            });

            Commands.Add(new Command(PlayerRights.Default, new string[] { "commands", "cmds" })
            {
                Execute = (player, args) => {
                    foreach (Command cmd in Commands.Where(c => c.HasRequiredRights(player.PersistentVars.Rights)))
                    {
                        PacketHandler.SendPacket(player, new SendMessage(string.Join(", ", cmd.Identifiers)));
                    }

                }
            });
            Commands.Add(new Command(PlayerRights.Default, new string[] { "fps" })
            {
                Execute = (player, args) => PacketHandler.SendPacket(player, new SendMessage("::fps"))
            });
            Commands.Add(new Command(PlayerRights.Default, new string[] { "censor" })
            {
                Execute = (player, args) => PacketHandler.SendPacket(player, new SendMessage("::censor"))
            });
            Commands.Add(new Command(PlayerRights.Default, new string[] { "ping" })
            {
                Execute = (player, args) => PacketHandler.SendPacket(player, new SendMessage("::ping"))
            });
            Commands.Add(new Command(PlayerRights.Default, new string[] { "shading" })
            {
                Execute = (player, args) => PacketHandler.SendPacket(player, new SendMessage("::shading"))
            });
            Commands.Add(new Command(PlayerRights.Default, new string[] { "memory" })
            {
                Execute = (player, args) => PacketHandler.SendPacket(player, new SendMessage("::memory"))
            });
            Commands.Add(new Command(PlayerRights.Default, new string[] { "data" })
            {
                Execute = (player, args) => PacketHandler.SendPacket(player, new SendMessage("::data"))
            });
            Commands.Add(new Command(PlayerRights.Default, new string[] { "players", "playercount" })
            {
                Execute = (player, args) => PacketHandler.SendPacket(player, 
                    new SendMessage("There are currently " + WorldHandler.World.Players.Entities.Count + " players online."))
            });
        }

    }
}
