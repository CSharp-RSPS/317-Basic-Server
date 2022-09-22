using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sets an interface's model rotation and zoom
    /// </summary>
    public sealed class SendInterfaceModelRotation : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(9);
            writer.WriteHeader(encryptor, 230);
            return writer;
        }

    }

}
