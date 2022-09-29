using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util.Annotations;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{

    /// <summary>
    /// This packet is sent when a player adds another player to their ignore list.
    /// </summary>
    [PacketInfo(133, 8)]
    public sealed class ReceiveAddIgnore : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            long playerId = reader.ReadLong();

            if (playerId < 0)
            {
                return;
            }
            player.Ignores.Add(playerId);
        }

    }
}
