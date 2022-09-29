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
    /// This packet is sent when a player removes another player from their ignore list.
    /// </summary>
    [PacketInfo(74, 8)]
    public sealed class ReceiveRemoveIgnore : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            long playerId = reader.ReadLong();
        }

    }
}
