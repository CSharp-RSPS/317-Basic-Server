using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Comms.Messaging;
using RSPS.Net.Connections;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using RSPS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
{

    /// <summary>
    /// This packet is sent when a player adds a friend to their friends list.
    /// </summary>
    [PacketInfo(188, 8)]
    public sealed class ReceiveAddFriend : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            long usernameAsLong = reader.ReadLong();

            if (usernameAsLong < 0)
            {
                return;
            }
            ContactsHandler.AddFriend(player, usernameAsLong);
        }

    }
}
