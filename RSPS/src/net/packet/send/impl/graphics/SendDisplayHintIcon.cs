using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Displays a hint icon.
    /// </summary>
    public sealed class SendDisplayHintIcon : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(1);
            writer.WriteVariableHeader(encryptor, 254);

            writer.FinishVariableHeader();
            return writer;
        }

    }

}
