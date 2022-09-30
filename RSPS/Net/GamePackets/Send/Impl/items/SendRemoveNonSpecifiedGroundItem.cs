using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
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
