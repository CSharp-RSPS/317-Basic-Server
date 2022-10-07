using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// This packet sends the first list load status.
    /// </summary>
    [PacketDef(SendPacketDefinition.FriendsListStatus)]
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
