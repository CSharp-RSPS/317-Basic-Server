using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.Mobiles.Players.Events;
using RSPS.Entities.Mobiles.Players.Events.Impl;
using RSPS.Game.Items;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
{

    /// <summary>
    /// This packet is sent when a player enters an X amount of items they want to bank.
    /// </summary>
    [PacketInfo(208, 4)]
    public sealed class ReceiveBankXItemsPt2 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int amount = reader.ReadInt();

            ItemAmountInputEvent? evt = player.PlayerEvents.FindActive<ItemAmountInputEvent>(PlayerEventType.ItemAmountInput);

            if (evt != null)
            {
                evt.SetAmount(amount);
                player.PlayerEvents.NextStage(player, evt);
            }
        }

    }
}
