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
using RSPS.Game.Comms.Dialogues;

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
            // Dialogues
            ButtonEvents.Add(14445, (player) => DialogueHandler.PickOption(player, 1)); // 2-choice dialogue, option 1
            ButtonEvents.Add(14446, (player) => DialogueHandler.PickOption(player, 2)); // 2-choice dialogue, option 2
            ButtonEvents.Add(2471, (player) => DialogueHandler.PickOption(player, 1)); // 3-choice dialogue, option 1
            ButtonEvents.Add(2472, (player) => DialogueHandler.PickOption(player, 2)); // 3-choice dialogue, option 2
            ButtonEvents.Add(2473, (player) => DialogueHandler.PickOption(player, 3)); // 3-choice dialogue, option 3
            ButtonEvents.Add(8209, (player) => DialogueHandler.PickOption(player, 1)); // 4-choice dialogue, option 1
            ButtonEvents.Add(8210, (player) => DialogueHandler.PickOption(player, 2)); // 4-choice dialogue, option 2
            ButtonEvents.Add(8211, (player) => DialogueHandler.PickOption(player, 3)); // 4-choice dialogue, option 3
            ButtonEvents.Add(8212, (player) => DialogueHandler.PickOption(player, 4)); // 4-choice dialogue, option 4
            ButtonEvents.Add(8221, (player) => DialogueHandler.PickOption(player, 1)); // 5-choice dialogue, option 1
            ButtonEvents.Add(8222, (player) => DialogueHandler.PickOption(player, 2)); // 5-choice dialogue, option 2
            ButtonEvents.Add(8223, (player) => DialogueHandler.PickOption(player, 3)); // 5-choice dialogue, option 3
            ButtonEvents.Add(8224, (player) => DialogueHandler.PickOption(player, 4)); // 5-choice dialogue, option 4
            ButtonEvents.Add(8225, (player) => DialogueHandler.PickOption(player, 5)); // 5-choice dialogue, option 5

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
