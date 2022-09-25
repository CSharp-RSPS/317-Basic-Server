using RSPS.src.entity.player;
using RSPS.src.net.packet.send;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using RSPS.src.net.packet.send.impl;

namespace RSPS.src.game.comms.commands
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
