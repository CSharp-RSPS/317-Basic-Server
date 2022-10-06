using RSPS.Entities.Mobiles.Players;
using RSPS.Game.UI;
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
    /// This packet is sent when a player casts magic (i.e. High Level Alchemy) on the items in their inventory.
    /// </summary>
    [PacketInfo(237, 8)]
    public sealed class ReceiveMagicOnItems : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int slot = reader.ReadShort(false);
            int itemId = reader.ReadShortAdditional();
            int interfaceId = reader.ReadShort(false);
            int spellId = reader.ReadShortAdditional();

            switch (interfaceId)
            {
                case Interfaces.Inventory: // Inventory
                    break;
            }
        }

    }
}
