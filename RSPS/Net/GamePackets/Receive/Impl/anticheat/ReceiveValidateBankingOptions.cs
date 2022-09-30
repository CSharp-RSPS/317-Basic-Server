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
    /// Validates banking options
    /// </summary>
    public sealed class ReceiveValidateBankingOptions : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {

        }

    }
}
