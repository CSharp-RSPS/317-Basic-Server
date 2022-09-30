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
    /// Send to validate walking.
    /// </summary>
    public sealed class ReceiveValidateWalking : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {

        }

    }
}
