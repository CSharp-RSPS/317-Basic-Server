using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Set cutscene camera
    /// </summary>
    [PacketDef(PacketDefinition.SetCutsceneCamera)]
    public sealed class SendSetCutsceneCamera : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            throw new NotImplementedException();
        }

    }

}
