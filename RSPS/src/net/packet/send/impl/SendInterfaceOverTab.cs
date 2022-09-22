using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Draws an interface over the tab area.
    /// </summary>
    public sealed class SendInterfaceOverTab : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(2);
            writer.WriteHeader(encryptor, 106);
            return writer;
        }

    }

}
