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
    /// Sent when a moderator or administrator selects the second option of a player.
    /// </summary>
    public sealed class ReceivePlayerOption2 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {

        }

    }
}
