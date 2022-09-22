using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sends the chat privacy settings.
    /// </summary>
    public sealed class SendChatSettings : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(4);
            writer.WriteHeader(encryptor, 206);
            return writer;
        }

    }

}
