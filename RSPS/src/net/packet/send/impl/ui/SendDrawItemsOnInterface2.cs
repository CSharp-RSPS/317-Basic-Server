using RSPS.src.Util.Annotations;
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
    [PacketDef(PacketDefinition.DrawItemsOnInterface2)]
    public sealed class SendDrawItemsOnInterface2 : IPacketVariablePayloadBuilder
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
