using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Delete ground item.
    /// </summary>
    public sealed class SendDeleteGroundItem : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(3);
            writer.WriteHeader(encryptor, 64);
            return writer;
        }

    }

}
