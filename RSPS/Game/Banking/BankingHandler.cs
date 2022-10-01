using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Banking
{
    /// <summary>
    /// Handles banking related operations
    /// </summary>
    public static class BankingHandler
    {

        /// <summary>
        /// Holds identifiers of objects in the game world that can be used to bank
        /// </summary>
        public static readonly int[] BankObjects = {
            2213, 2693, 3045, 3194, 4483, 5276, 6084, 10517, 10562, 11338, 11758
            , 65445, 65436, 65476, 65518
        };

        /// <summary>
        /// Opens the bank for a player
        /// </summary>
        /// <param name="player">The player</param>
        public static void OpenBank(Player player)
        {
            PacketHandler.SendPacket(player, new SendInventoryOverlay(5292, 5063));
            player.Bank.RefreshUI(player);
            PacketHandler.SendPacket(player, new SendDrawItemsOnInterface2(5064, player.Bank.Items));
            PacketHandler.SendPacket(player, new SendDrawItemsOnInterface2(5064, player.Inventory.Items));
        }

    }
}
