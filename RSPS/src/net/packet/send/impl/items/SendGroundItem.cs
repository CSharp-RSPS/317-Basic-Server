using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Place an item stack on the ground.
    /// </summary>
    public sealed class SendGroundItem : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(6);
            writer.WriteHeader(encryptor, 44);
            return writer;
        }

    }

}
