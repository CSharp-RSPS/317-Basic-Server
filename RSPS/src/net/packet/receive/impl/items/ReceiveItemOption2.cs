using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{

    /// <summary>
    /// This packet is sent when a player clicks the alternate second option of an item.
    /// </summary>
    [PacketInfo(16, 6)]
    public sealed class ReceiveItemOption2 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int itemId = reader.ReadShortAdditional();
            int slot = reader.ReadShortAdditionalLittleEndian();
            int interfaceId = reader.ReadShortAdditionalLittleEndian();

            switch (interfaceId)
            {
                case 3214: // Inventory
                    break;
            }
        }

    }
}
