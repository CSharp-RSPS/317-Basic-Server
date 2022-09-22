using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sends an object spawn request to the client.
    /// </summary>
    public sealed class SendObjectSpawn : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(6);
            writer.WriteHeader(encryptor, 151);
            return writer;
        }

    }

}
