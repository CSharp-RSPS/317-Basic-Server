using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Draw a collection of items on an interface.
    /// </summary>
    public sealed class SendDrawItemsOnInterface : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(1);
            writer.WriteVariableShortHeader(encryptor, 34);

            writer.FinishVariableShortHeader();
            return writer;
        }

    }

}
