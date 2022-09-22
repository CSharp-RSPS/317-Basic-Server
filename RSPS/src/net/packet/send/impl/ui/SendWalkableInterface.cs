using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Displays an interface in walkable mode.
    /// </summary>
    public sealed class SendWalkableInterface : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(3);
            writer.WriteHeader(encryptor, 208);
            return writer;
        }

    }

}
