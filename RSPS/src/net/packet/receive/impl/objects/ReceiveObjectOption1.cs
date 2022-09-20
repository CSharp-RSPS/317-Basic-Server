using RSPS.src.entity.player;
using RSPS.src.net.packet.send.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{

    /// <summary>
    /// Sent when the player clicks the first option of an object, such as "Cut" for trees.
    /// </summary>
    public sealed class ReceiveObjectOption1 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader packetReader)
        {

        }

    }
}
