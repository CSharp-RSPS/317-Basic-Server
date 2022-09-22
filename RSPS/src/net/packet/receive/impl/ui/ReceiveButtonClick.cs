using RSPS.src.entity.player;
using RSPS.src.net.Connections;
using RSPS.src.net.packet.send.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    /// <summary>
    /// Sent when a player clicks an in-game button.
    /// </summary>
    public sealed class ReceiveButtonClick : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader packetReader)
        {
            //int buttonId = PacketReader.HexToInt(packetReader.ReadBytes(2));
            int buttonId = packetReader.ReadShort();

            if (buttonId < 0)
            {
                return;
            }
            switch (buttonId)
            {
                case 9154:
                    PlayerManager.Logout(player);
                    break;

                case 52053:
                    Console.WriteLine("Head Banging");
                    break;

                case 152:
                    player.MovementHandler.RunToggled = !player.MovementHandler.RunToggled;
                    PacketHandler.SendPacket(player, new SendConfiguration(173, player.MovementHandler.RunToggled));
                    Console.WriteLine("Is player run toggled? " + player.MovementHandler.RunToggled);
                    break;

                default:
                    Console.WriteLine("Unhandled button Id: " + buttonId);
                    break;
            }
        }
    }
}
