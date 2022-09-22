using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Disconnects the client from the server.
    /// </summary>
    public sealed class SendLogout : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(1);
            writer.WriteHeader(encryptor, 109);
            return writer;
        }

    }

}
