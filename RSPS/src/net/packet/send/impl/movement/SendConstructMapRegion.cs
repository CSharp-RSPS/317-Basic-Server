using RSPS.src.Util.Annotations;
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
    [PacketDef(PacketDefinition.ConstructMapRegion)]
    public sealed class SendConstructMapRegion : IPacketVariablePayloadBuilder
    {


        public int GetPayloadSize()
        {
            throw new NotImplementedException();
        }

        public void WritePayload(PacketWriter writer)
        {
            throw new NotImplementedException();
        }

    }

}
