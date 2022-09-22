using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sets the offset for drawing of an interface.
    /// </summary>
    public sealed class SendInterfaceOffset : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(7);
            writer.WriteHeader(encryptor, 70);
            return writer;
        }

    }

}
