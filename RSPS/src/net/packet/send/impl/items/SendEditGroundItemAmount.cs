using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Edit ground item amount
    /// </summary>
    public sealed class SendEditGroundItemAmount : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(8);
            writer.WriteHeader(encryptor, 84);
            return writer;
        }

    }

}
