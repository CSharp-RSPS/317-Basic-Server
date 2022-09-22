using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Friends list load status.
    /// </summary>
    public sealed class SendFriendsListStatus : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(2);
            writer.WriteHeader(encryptor, 221);
            return writer;
        }

    }

}
