using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Changes the color of an interface.
    /// </summary>
    public sealed class SendInterfaceColor : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(5);
            writer.WriteHeader(encryptor, 122);
            return writer;
        }

    }

}
