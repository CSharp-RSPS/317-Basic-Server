using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Shows an interface in the chat box??????
    /// </summary>
    public sealed class SendSpawnAnimatedObject : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(5);
            writer.WriteHeader(encryptor, 16);
            return writer;
        }

    }

}
