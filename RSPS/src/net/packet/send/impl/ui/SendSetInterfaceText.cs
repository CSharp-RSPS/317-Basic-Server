using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Attaches text to an interface.
    /// </summary>
    public sealed class SendSetInterfaceText : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(1);
            writer.WriteVariableShortHeader(encryptor, 126);

            writer.FinishVariableShortHeader();
            return writer;
        }

    }

}
