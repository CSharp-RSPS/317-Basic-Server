using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Begin processing position related packets.
    /// </summary>
    public sealed class SendBeginProcessingPositionRelatedPackets : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(1);
            writer.WriteVariableShortHeader(encryptor, 60);

            writer.FinishVariableShortHeader();
            return writer;
        }

    }

}
