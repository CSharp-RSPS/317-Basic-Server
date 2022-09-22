using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Spin camera
    /// </summary>
    public sealed class SendSpinCamera : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(7);
            writer.WriteHeader(encryptor, 166);
            return writer;
        }

    }

}
