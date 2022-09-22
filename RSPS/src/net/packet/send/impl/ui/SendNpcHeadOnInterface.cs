using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Place the head of an NPC on an interface
    /// </summary>
    public sealed class SendNpcHeadOnInterface : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(5);
            writer.WriteHeader(encryptor, 75);
            return writer;
        }

    }

}
