using RSPS.src.entity.player;
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
    /// This packet is sent when a player casts magic (i.e. High Level Alchemy) on the items in their inventory.
    /// </summary>
    [PacketInfo(237, 8)]
    public sealed class ReceiveMagicOnItems : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int slot = reader.ReadShort();
            int itemId = reader.ReadShort(Packet.ValueType.Additional);
            int interfaceId = reader.ReadShort();
            int spellId = reader.ReadShort(Packet.ValueType.Additional);

            switch (interfaceId)
            {
                case 3214: // Inventory
                    break;
            }
        }

    }
}
