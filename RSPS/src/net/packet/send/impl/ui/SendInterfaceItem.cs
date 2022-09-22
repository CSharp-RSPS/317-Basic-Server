using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Displays an item model inside an interface.
    /// </summary>
    public sealed class SendInterfaceItem : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(7);
            writer.WriteHeader(encryptor, 246);
            return writer;
        }

    }

}
