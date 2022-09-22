using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sends an object removal request to the client.
    /// </summary>
    public sealed class SendObjectRemoval : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(4);
            writer.WriteHeader(encryptor, 101);
            return writer;
        }

    }

}
