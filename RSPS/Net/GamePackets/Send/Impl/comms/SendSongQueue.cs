using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// This packet queue's a song to be played next. The client then proceeds to request the queued song using the on-demand protocol.
    /// </summary>
    [PacketDef(PacketDefinition.SongQueue)]
    public sealed class SendSongQueue : IPacketPayloadBuilder
    {

        /// <summary>
        /// The song ID
        /// </summary>
        public int SongId { get; private set; }

        /// <summary>
        /// The previous song ID
        /// </summary>
        public int PreviousSongId { get; private set; }


        /// <summary>
        /// Creates a new song queue packet payload builder
        /// </summary>
        /// <param name="songId">The song ID</param>
        /// <param name="previousSongId">The previous song ID</param>
        public SendSongQueue(int songId, int previousSongId)
        {
            SongId = songId;
            PreviousSongId = previousSongId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortAdditionalLittleEndian(SongId);
            writer.WriteShortAdditionalLittleEndian(PreviousSongId);
        }

    }

}
