using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sends how much run energy the player currently has.
    /// </summary>
    [PacketDef(PacketDefinition.RunEnergy)]
    public sealed class SendRunEnergy : IPacketPayloadBuilder
    {

        /// <summary>
        /// The energy level
        /// </summary>
        public int Energy { get; private set; }

        /// <summary>
        /// Creates a new run energy packet payload builder
        /// </summary>
        /// <param name="energy">The energy level</param>
        public SendRunEnergy(int energy)
        {
            Energy = energy;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte((int)Math.Floor(Energy * 0.01));//Server stored energy is 100x greater than clients
        }

    }
}
