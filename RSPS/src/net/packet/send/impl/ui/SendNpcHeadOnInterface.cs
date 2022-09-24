using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Place the head of an NPC on an interface
    /// </summary>
    [PacketDef(PacketDefinition.NPCHeadOnInterface)]
    public sealed class SendNpcHeadOnInterface : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            throw new NotImplementedException();
        }

    }

}
