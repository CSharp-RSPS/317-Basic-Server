using RSPS.Game.Comms.Dialogues;
using RSPS.Game.Items;
using RSPS.Game.Items.Containers;
using RSPS.Game.Trading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Mobiles.Players.Variables
{
    /// <summary>
    /// Holds non-persistent player variables that are used globally
    /// </summary>
    public sealed class NonPersistentVariables
    {

        /// <summary>
        /// The identifier of the interface the player has open, if any
        /// </summary>
        public int OpenInterfaceId { get; set; } = -1;

        /// <summary>
        /// Whether the player has an interface opened
        /// </summary>
        public bool HasOpenInterface => OpenInterfaceId != -1;

        /// <summary>
        /// Whether to withdraw items from the bank noted
        /// </summary>
        public bool NotedBanking { get; set; }

        /// <summary>
        /// The active trade container
        /// </summary>
        public ItemContainer? TradeContainer { get; set; }

        /// <summary>
        /// The current trade partner if any
        /// </summary>
        public Player? TradePartner { get; set; }

        /// <summary>
        /// The trade stage we're in
        /// </summary>
        public TradeStage? TradeStage { get; set; }

        /// <summary>
        /// Retrieves whether we're trading at the moment
        /// </summary>
        public bool IsTrading => TradeContainer != null && TradePartner != null && TradeStage != null;

        /// <summary>
        /// The current dialogue the player is in
        /// </summary>
        public Dialogue? CurrentDialogue { get; set; }

        /// <summary>
        /// The item the player wants to destroy
        /// </summary>
        public Item? DestroyItem { get; set; }

        /// <summary>
        /// Whether the game has focus from the player
        /// </summary>
        public bool HasFocus { get; set; }

    }
}
