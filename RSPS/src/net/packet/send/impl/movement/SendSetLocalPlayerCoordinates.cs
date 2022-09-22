using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Set local player coordinates
    /// </summary>
    public sealed class SendSetLocalPlayerCoordinates : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(3);
            writer.WriteHeader(encryptor, 85);
            return writer;
        }

    }

}
