using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Remove non-specified ground items??????
    /// </summary>
    [PacketDef(PacketDefinition.RemoveNonSpecifiedGroundItems)]
    public sealed class SendRemoveNonSpecifiedGroundItem : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            throw new NotImplementedException();
        }

    }

}
