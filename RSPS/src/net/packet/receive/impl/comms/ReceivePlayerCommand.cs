using RSPS.src.entity.player;
using RSPS.src.net.packet.send.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    /// <summary>
    /// Sent when the player enters a command in the chat box (e.g. "::command")
    /// </summary>
    public sealed class ReceivePlayerCommand : IReceivePacket
    {


        public void ReceivePacket(Player player, int packetOpcode, int packetSize, PacketReader packetReader)
        {
            string input = packetReader.ReadString().Trim().ToLower();

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

            //resetanim
            PacketHandler.SendPacket(player.PlayerConnection, new SendAnimationReset());
            //TODO handle command
        }

    }
}
