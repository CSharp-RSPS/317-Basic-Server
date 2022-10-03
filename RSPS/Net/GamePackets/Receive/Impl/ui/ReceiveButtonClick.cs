using RSPS.Entities.Mobiles.Players;
using RSPS.Net.Connections;
using RSPS.Net.GamePackets.Send;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
{
    /// <summary>
    /// This is sent when a player clicks a button in-game, with the id of the button being clicked.
    /// </summary>
    [PacketInfo(185, 2)]
    public sealed class ReceiveButtonClick : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int buttonId = reader.ReadShort();

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
                    player.PlayerMovement.Running = !player.PlayerMovement.Running;
                    PacketHandler.SendPacket(player, new SendConfiguration(173, player.PlayerMovement.Running));
                    Console.WriteLine("Is player run toggled? " + player.PlayerMovement.Running);
                    break;

                case 5386:
                    player.NonPersistentVars.NotedBanking = true;
                    break;

                case 5387:
                    player.NonPersistentVars.NotedBanking = false;
                    break;

                case 8130:
                    // Bank rearrange mode: swap
                    break;

                case 8131:
                    // Bank rearrange mode: insert
                    break;

                default:
                    Debug.WriteLine("Unhandled button Id: " + buttonId);
                    break;
            }
        }
    }
}
