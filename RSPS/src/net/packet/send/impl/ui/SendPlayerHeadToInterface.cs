using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sends the players head model to an interface
    /// </summary>
    public sealed class SendPlayerHeadToInterface : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(3);
            writer.WriteHeader(encryptor, 185);
            return writer;
        }

    }

}
