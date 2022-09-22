using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Player to object transformation
    /// </summary>
    public sealed class SendPlayerToObjectTransformation : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(15);
            writer.WriteHeader(encryptor, 147);
            return writer;
        }

    }

}
