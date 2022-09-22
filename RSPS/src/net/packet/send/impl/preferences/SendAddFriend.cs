using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sends a friend to the friend list.
    /// </summary>
    public sealed class SendAddFriend : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(10);
            writer.WriteHeader(encryptor, 50);
            return writer;
        }

    }

}
