using RSPS.Entities.Mobiles.Players;
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
    /// This packet is sent when a player removes a friend from their friends list.
    /// </summary>
    [PacketInfo(215, 8)]
    public sealed class ReceiveRemoveFriend : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            long playerId = reader.ReadLong();
        }

    }
}
