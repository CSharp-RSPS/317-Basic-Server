using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sends the player membership flag and player list index.
    /// </summary>
    [PacketDef(PacketDefinition.InitializePlayer)]
    public sealed class SendInitializePlayer : IPacketPayloadBuilder
    {

        /// <summary>
        /// Whether the player is a member
        /// </summary>
        public bool Member { get; private set;  }

        /// <summary>
        /// The index of the player in the players list
        /// </summary>
        public int PlayerListIndex { get; private set; }


        /// <summary>
        /// Initializes a player
        /// </summary>
        /// <param name="member">Whether the player is a member</param>
        /// <param name="playerListIndex">The index of the player in the players list</param>
        public SendInitializePlayer(bool member, int playerListIndex)
        {
            Member = member;
            PlayerListIndex = playerListIndex;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(Member ? 1 : 0, Packet.ValueType.Additional);
            writer.WriteShort(PlayerListIndex, Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
        }

    }

}
