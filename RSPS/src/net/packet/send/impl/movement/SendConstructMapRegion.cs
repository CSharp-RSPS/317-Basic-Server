using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Constructs a dynamic map region using a palette of 8*8 tiles.
    /// </summary>
    public sealed class SendConstructMapRegion : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(1);
            writer.WriteVariableShortHeader(encryptor, 241);

            writer.FinishVariableShortHeader();
            return writer;
        }

    }

}
