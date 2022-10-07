using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl.comms
{
    /// <summary>
    /// Sets what audio/sound is to play at a certain moment.
    /// </summary>
    [PacketDef(SendPacketDefinition.Audio)]
    public sealed class SendAudio : IPacketPayloadBuilder
    {

        /// <summary>
        /// The sound ID
        /// </summary>
        public int SoundId { get; private set; }

        /// <summary>
        /// The volume
        /// </summary>
        public int Volume { get; private set; }

        /// <summary>
        /// The play delay
        /// </summary>
        public int Delay { get; private set; }


        /// <summary>
        /// Creates a new audio payload builder
        /// </summary>
        /// <param name="soundId">The sound ID</param>
        /// <param name="volume">The volume</param>
        /// <param name="delay">The play delay</param>
        public SendAudio(int soundId, int volume, int delay)
        {
            SoundId = soundId;
            Volume = volume;
            Delay = delay;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(SoundId);
            writer.WriteByte(Volume);
            writer.WriteShort(Delay);
        }

    }
}
