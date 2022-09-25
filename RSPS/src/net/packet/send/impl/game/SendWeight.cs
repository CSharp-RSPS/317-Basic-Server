using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sends how much weight of equipment the player is wearing (e.g. Rune plate-body is 9.04kg).
    /// </summary>
    [PacketDef(PacketDefinition.Weight)]
    public sealed class SendWeight : IPacketPayloadBuilder
    {

        /// <summary>
        /// The weight
        /// </summary>
        public short Weight { get; private set; }


        /// <summary>
        /// Creates a new weight packet payload builder
        /// </summary>
        /// <param name="weight">The weight</param>
        public SendWeight(short weight)
        {
            Weight = weight;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(Weight);
        }

    }

}
