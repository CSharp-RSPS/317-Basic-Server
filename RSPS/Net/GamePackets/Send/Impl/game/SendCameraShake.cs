using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Causes the camera to shake.
    /// </summary>
    [PacketDef(PacketDefinition.CameraShake)]
    public sealed class SendCameraShake : IPacketPayloadBuilder
    {

        /// <summary>
        /// The parameter (camera X, Y, Z, jaw, pitch)
        /// </summary>
        public int Parameter { get; private set; }

        /// <summary>
        /// The jitter for randomization
        /// </summary>
        public int Jitter { get; private set; }

        /// <summary>
        /// The amplitude
        /// </summary>
        public int Amplitude { get; private set; }

        /// <summary>
        /// The frequency (scaled by 100)
        /// </summary>
        public int Frequency { get; private set; }


        /// <summary>
        /// Creates a new camera shake payload writer
        /// </summary>
        /// <param name="parameter">The parameter (camera X, Y, Z, jaw, pitch)</param>
        /// <param name="jitter">The jitter for randomization</param>
        /// <param name="amplitude">The amplitude</param>
        /// <param name="frequency">The frequency (scaled by 100)</param>
        public SendCameraShake(int parameter, int jitter, int amplitude, int frequency)
        {
            Parameter = parameter;
            Jitter = jitter;
            Amplitude = amplitude;
            Frequency = frequency;
        }   

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(Parameter);
            writer.WriteByte(Jitter);
            writer.WriteByte(Amplitude);
            writer.WriteByte(Frequency);
        }

    }
}
