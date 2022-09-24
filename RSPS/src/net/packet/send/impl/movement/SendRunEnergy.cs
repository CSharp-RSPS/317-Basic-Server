using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sends the players run energy level.
    /// </summary>
    [PacketDef(PacketDefinition.RunEnergy)]
    public sealed class SendRunEnergy : IPacketPayloadBuilder
    {

        private int energy;

        public SendRunEnergy(int energy)
        {
            this.energy = energy;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte((int)Math.Floor(energy * 0.01));//Server stored energy is 100x greater than clients
        }
    }
}
