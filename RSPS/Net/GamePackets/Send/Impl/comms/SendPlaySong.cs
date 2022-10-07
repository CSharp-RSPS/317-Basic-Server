using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Sending this packet to the client will cause the client to start playing a song.
    /// </summary>
    [PacketDef(SendPacketDefinition.PlaySong)]
    public sealed class SendPlaySong : IPacketPayloadBuilder
    {

        /// <summary>
        /// The song ID
        /// </summary>
        public int SongId { get; private set; }


        /// <summary>
        /// Creates a new play song packet payload builder
        /// </summary>
        /// <param name="songId">The song ID</param>
        public SendPlaySong(int songId)
        {
            SongId = songId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortLittleEndian(SongId);
        }

    }

}
