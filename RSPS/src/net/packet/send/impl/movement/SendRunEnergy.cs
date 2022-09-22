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
    public sealed class SendRunEnergy : ISendPacket
    {

        private int energy;

        public SendRunEnergy(int energy)
        {
            this.energy = energy;
        }

        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter packetWriter = Packet.CreatePacketWriter(2);
            packetWriter.WriteHeader(encryptor, 110);//221
            packetWriter.WriteByte((int)Math.Floor(energy * 0.01));//Server stored energy is 100x greater than clients
            return packetWriter;
        }
    }
}
