using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.Mobiles.Players.Events.Impl;
using RSPS.Net.GamePackets.Send;
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
    /// This packet is sent when a player requests to bank an X amount of items.
    /// </summary>
    [PacketInfo(135, 6)]
    public sealed class ReceiveBankXItemsPt1 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int slotId = reader.ReadShortLittleEndian();
            int interfaceId = reader.ReadShortAdditional();
            int itemId = reader.ReadShortLittleEndian();

            if (interfaceId < 0 || itemId < 0 || slotId < 0)
            {
                return;
            }
            player.PlayerEvents.Start(player, new ItemAmountInputEvent(slotId, interfaceId, itemId));
        }

    }
}
