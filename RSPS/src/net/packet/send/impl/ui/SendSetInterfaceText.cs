using RSPS.src.Util.Annotations;
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
    [PacketDef(PacketDefinition.SetInterfaceText)]
    public sealed class SendSetInterfaceText : IPacketVariablePayloadBuilder
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
