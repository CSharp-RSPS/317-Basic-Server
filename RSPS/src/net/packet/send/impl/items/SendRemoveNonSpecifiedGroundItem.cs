using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Remove non-specified ground items??????
    /// </summary>
    public sealed class SendRemoveNonSpecifiedGroundItem : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(4);
            writer.WriteHeader(encryptor, 156);
            return writer;
        }

    }

}
