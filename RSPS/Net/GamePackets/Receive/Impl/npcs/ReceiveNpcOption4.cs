using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets.Send.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
{

    /// <summary>
    /// Sent when a player clicks the fourth option of an NPC.
    /// </summary>
    public sealed class ReceiveNpcOption4 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {

        }

    }
}
