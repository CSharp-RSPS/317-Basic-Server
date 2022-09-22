using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Causes a sidebar icon to start flashing.
    /// </summary>
    public sealed class SendFlashSidebar : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(2);
            writer.WriteHeader(encryptor, 24);
            return writer;
        }

    }

}
