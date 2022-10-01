using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Comms.Messaging;
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
    /// This packet is sent when a player removes another player from their ignore list.
    /// </summary>
    [PacketInfo(74, 8)]
    public sealed class ReceiveRemoveIgnore : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            long usernameAsLong = reader.ReadLong();

            if (usernameAsLong <= 0)
            {
                return;
            }
            ContactsHandler.RemoveIgnore(player, usernameAsLong);
        }

    }
}
