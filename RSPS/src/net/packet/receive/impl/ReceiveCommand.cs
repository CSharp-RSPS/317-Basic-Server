using RSPS.src.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    public sealed class ReceiveCommand : IReceivePacket
    {


        public void ReceivePacket(Player player, int packetOpCode, int packetLength, PacketReader packetReader)
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
            //TODO handle command
        }

    }
}
