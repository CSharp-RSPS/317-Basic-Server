using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Sends friend data to the client
    /// Attempts to update player node, if player isn't in the friends list and there is space, the player is added to the friend list.
    /// </summary>
    [PacketDef(PacketDefinition.SendAddFriend)]
    public sealed class SendAddFriend : IPacketPayloadBuilder
    {

        /// <summary>
        /// The name of the player as long value
        /// </summary>
        public long PlayerName { get; private set; }

        /// <summary>
        /// The world status meaning the ID of the world if online, 0 when logged out
        /// </summary>
        public int WorldStatus { get; private set; }


        /// <summary>
        /// Creates a new add friend packet payload builder
        /// </summary>
        /// <param name="playerName">The name of the player as long value</param>
        /// <param name="worldStatus"></param>
        public SendAddFriend(long playerName, int worldStatus)
        {
            PlayerName = playerName;
            WorldStatus = worldStatus;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteLong(PlayerName);
            writer.WriteByte(WorldStatus + 1); // >= 2 = logged in, <= 1 = logged out
        }

    }

}
