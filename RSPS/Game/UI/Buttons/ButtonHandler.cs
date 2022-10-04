using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Trading;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.UI.Buttons
{
    /// <summary>
    /// Handles game button related operations
    /// </summary>
    public static class ButtonHandler
    {

        /// <summary>
        /// Holds the possible button events
        /// </summary>
        public static readonly Dictionary<int, ButtonAction> ButtonEvents = new();

        static ButtonHandler()
        {
            ButtonEvents.Add(152, (player) =>
            {
                player.PlayerMovement.Running = !player.PlayerMovement.Running;
                PacketHandler.SendPacket(player, new SendConfiguration(173, player.PlayerMovement.Running));
            });

            // Trading
            ButtonEvents.Add(3420, (player) => TradingHandler.ConfirmTrade(player)); // Confirm trade
            ButtonEvents.Add(3546, (player) => TradingHandler.AcceptTrade(player)); // Accept trade

            // Banking
            ButtonEvents.Add(5386, (player) => player.NonPersistentVars.NotedBanking = true); // Switch to noted mode for unbanking
            ButtonEvents.Add(5387, (player) => player.NonPersistentVars.NotedBanking = false); // Switch to unnoted mode for unbanking
            ButtonEvents.Add(8130, (player) => player.LoggedIn = player.LoggedIn);  // TODO: Bank rearrange mode: swap
            ButtonEvents.Add(8131, (player) => player.LoggedIn = player.LoggedIn); // TODO: Bank rearrange mode: insert

            // Misc
            ButtonEvents.Add(9154, (player) => PlayerManager.Logout(player)); // Logout of the game

        }

        /// <summary>
        /// Handles a button action for a player
        /// </summary>
        /// <param name="player">The player</param>
        public delegate void ButtonAction(Player player);

        /// <summary>
        /// Handles a button click for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="buttonId">The button identifier</param>
        /// <returns>Whether any buttons were handled</returns>
        public static void HandleButtonClick(Player player, int buttonId)
        {
            if (!ButtonEvents.ContainsKey(buttonId))
            {
                Debug.WriteLine("Unhandled button: " + buttonId);
                return;
            }
            ButtonEvents[buttonId].Invoke(player);
        }

    }
}
