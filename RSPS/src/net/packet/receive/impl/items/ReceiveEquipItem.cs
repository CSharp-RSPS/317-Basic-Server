using RSPS.src.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    /// <summary>
    /// Sent when a player equips an item.
    /// </summary>
    public sealed class ReceiveEquipItem : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader packetReader)
        {

        }

    }
}
