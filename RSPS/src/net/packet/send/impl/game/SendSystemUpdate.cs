using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sends how many seconds until a 'System Update.'
    /// </summary>
    public sealed class SendSystemUpdate : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(3);
            writer.WriteHeader(encryptor, 114);
            return writer;
        }

    }

}
