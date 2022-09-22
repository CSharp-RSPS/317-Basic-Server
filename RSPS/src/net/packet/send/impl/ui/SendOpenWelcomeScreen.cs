using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Displays the welcome screen.
    /// </summary>
    public sealed class SendOpenWelcomeScreen : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(11);
            writer.WriteHeader(encryptor, 176);
            return writer;
        }

    }

}
