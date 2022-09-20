using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    public sealed class SendAnimationReset : ISendPacket
    {
        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(0);
            writer.WriteHeader(encryptor, 1);
            return writer;
        }
    }
}
