﻿using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// This packet sends the first list load status.
    /// </summary>
    [PacketDef(PacketDefinition.FriendsListStatus)]
    public sealed class SendFriendsListStatus : IPacketPayloadBuilder
    {

        /// <summary>
        /// The friends list status
        /// </summary>
        public int FriendsListStatus { get; private set; }


        /// <summary>
        /// Creates a new friends list status payload builder
        /// </summary>
        /// <param name="friendsListStatus">The friends list status</param>
        public SendFriendsListStatus(int friendsListStatus)
        {
            FriendsListStatus = friendsListStatus;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(FriendsListStatus);
        }

    }

}
