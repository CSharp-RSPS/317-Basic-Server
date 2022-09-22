using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Queues a song to be played next.
    /// </summary>
    public sealed class SendSongQueue : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(5);
            writer.WriteHeader(encryptor, 121);
            return writer;
        }

    }

}
