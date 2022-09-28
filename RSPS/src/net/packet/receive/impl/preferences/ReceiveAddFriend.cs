using RSPS.src.entity.player;
using RSPS.src.net.Connections;
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
    /// This packet is sent when a player adds a friend to their friends list.
    /// </summary>
    [PacketInfo(188, 8)]
    public sealed class ReceiveAddFriend : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            long playerId = reader.ReadLong();

            if (playerId < 0)
            {
                return;
            }
            player.Friends.Add(playerId);
        }

    }
}
