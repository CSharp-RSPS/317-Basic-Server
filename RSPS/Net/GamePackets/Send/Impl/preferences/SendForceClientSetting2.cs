using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Forcefully changes a client's setting's value. Also changes the default value for that setting.
    /// </summary>
    [PacketDef(PacketDefinition.ForceClientSetting2)]
    public sealed class SendForceClientSetting2 : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortLittleEndian(0); // config id
            writer.WriteIntInverseMiddleEndian(0); // the value
        }

    }

}
