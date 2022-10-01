using RSPS.Game.Items;
using RSPS.Game.Items.Containers;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Mobiles.Players.Events.Impl
{
    /// <summary>
    /// Represents an item amount input event
    /// </summary>
    public sealed class ItemAmountInputEvent : StagePlayerEvent
    {

        /// <summary>
        /// The item slot
        /// </summary>
        public int Slot { get; private set; }

        /// <summary>
        /// The interface identifier
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// The item identifier
        /// </summary>
        public int ItemId { get; private set; }

        /// <summary>
        /// The item amount received from input
        /// </summary>
        public int Amount { get; private set; }


        /// <summary>
        /// Creates a new item amount input event
        /// </summary>
        /// <param name="slot">The item slot</param>
        /// <param name="interfaceId">The interface identifier</param>
        /// <param name="itemId">The item identifier</param>
        public ItemAmountInputEvent(int slot, int interfaceId, int itemId)
            : base(PlayerEventType.ItemAmountInput, 2)
        {
            Slot = slot;
            InterfaceId = interfaceId;
            ItemId = itemId;
        }

        public ItemAmountInputEvent SetAmount(int amount)
        {
            Amount = amount;
            return this;
        }

        protected override bool ExecuteStage(Player player, int stage)
        {
            ItemContainer? itemContainer = null;

            switch (InterfaceId)
            {
                case 5064: // Bank inventory overlay
                    itemContainer = player.Inventory;
                    break;

                case 5382: // Bank
                    break;

                case 3900: // Shop
                    break;

                case 3823: // Shop inventory overlay
                    itemContainer = player.Inventory;
                    break;

                case 3322: // Trade inventory overlay
                    itemContainer = player.Inventory;
                    break;

                case 3415: // Trade
                    break;
            }
            if (itemContainer == null)
            {
                return true;
            }
            Item? item = itemContainer.GetItemBySlot(Slot);

            if (item == null || item.Id != ItemId)
            {
                return true;
            }
            switch (stage)
            {
                case 0:
                    player.NonPersistentVars.OpenInterfaceId = InterfaceId;
                    PacketHandler.SendPacket(player, PacketDefinition.InputAmount);
                    return false;

                case 1:
                    if (Amount <= 0)
                    {
                        return true;
                    }
                    //TODO
                    return true;

                default:
                    return true;
            }
        }

        protected override void Cancel()
        {
            // TODO close interfaces?
        }

    }
}
