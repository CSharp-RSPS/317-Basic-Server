using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Edit ground item amount
    /// </summary>
    [PacketDef(SendPacketDefinition.EditGroundItemAmount)]
    public sealed class SendEditGroundItemAmount : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            // ?
            writer.WriteByte(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
            writer.WriteShort(0);
        }

    }

}
