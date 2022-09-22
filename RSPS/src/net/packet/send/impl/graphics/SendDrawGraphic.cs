using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Draw a graphic at a given x/y position after a delay.
    /// </summary>
    public sealed class SendDrawGraphic : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(7);
            writer.WriteHeader(encryptor, 4);
            return writer;
        }

    }

}
