using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Creates a projectile.
    /// </summary>
    public sealed class SendCreateProjectile : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(1);
            writer.WriteHeader(encryptor, 117);
            return writer;
        }

    }

}
