using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Forcefully changes a client's setting's value. Also changes the default value for that setting.
    /// </summary>
    [PacketDef(PacketDefinition.ForceClientSetting2)]
    public sealed class SendForceClientSetting2 : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            throw new NotImplementedException();
        }

    }

}
