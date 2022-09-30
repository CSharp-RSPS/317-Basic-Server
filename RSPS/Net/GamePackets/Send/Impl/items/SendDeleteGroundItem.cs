using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Delete ground item.
    /// </summary>
    [PacketDef(PacketDefinition.DeleteGroundItem)]
    public sealed class SendDeleteGroundItem : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            throw new NotImplementedException();
        }

    }

}
