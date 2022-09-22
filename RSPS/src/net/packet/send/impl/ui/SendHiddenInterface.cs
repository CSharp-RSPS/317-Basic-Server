using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sets an interface to be hidden until hovered over.
    /// </summary>
    public sealed class SendHiddenInterface : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(4);
            writer.WriteHeader(encryptor, 171);
            return writer;
        }

    }

}
