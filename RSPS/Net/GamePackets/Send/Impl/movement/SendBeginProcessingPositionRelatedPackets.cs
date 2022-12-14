using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Begin processing position related packets.
    /// </summary>
    [PacketDef(SendPacketDefinition.BeginProcessingPositionRelatedPackets)]
    public sealed class SendBeginProcessingPositionRelatedPackets : IPacketVariablePayloadBuilder
    {


        public int GetPayloadSize()
        {
            throw new NotImplementedException();
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(0); // ?
            writer.WriteByteNegated(0); // ?

            for (int i = 0; i < 5; i++) // while inStream.currentOffset < pktSize
            {
                writer.WriteByte(0); // ?
            }
        }

    }

}
