using RSPS.src.entity.player;
using RSPS.src.game.comms.commands;
using RSPS.src.net.packet.send;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    /// <summary>
    /// This packet is sent when a player types a message with the prefix '::', 
    /// the message is then sent to the server and an appropriate action is taken (e.g. spawning an item).
    /// </summary>
    [PacketInfo(103, PacketHeaderType.VariableByte)]
    public sealed class ReceivePlayerCommand : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            string input = reader.ReadString().Trim().ToLower();

            if (string.IsNullOrEmpty(input))
            {
                return;
            }
            string[] arguments = input.Split(" ");

            if (arguments.Length <= 0)
            {
                return;
            }
            Console.WriteLine("Command: " + input);

            CommandHandler.HandleCommand(player, arguments[0], arguments);
        }

    }
}
