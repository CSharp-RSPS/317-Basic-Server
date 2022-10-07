using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Player to object transformation
    /// </summary>
    [PacketDef(SendPacketDefinition.PlayerToObjectTransformation)]
    public sealed class SendPlayerToObjectTransformation : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByteSubtrahend(0); // position offset?
            writer.WriteShort(0); // ?
            writer.WriteByteSubtrahend(0); // ?
            writer.WriteShortLittleEndian(0); // player index
            writer.WriteByteNegated(0); // ?
            writer.WriteShort(0); // ?
            writer.WriteByteSubtrahend(0); // ?
            writer.WriteByte(0);
            writer.WriteShort(0); // object id
            writer.WriteByteNegated(0);
        }

    }

}
