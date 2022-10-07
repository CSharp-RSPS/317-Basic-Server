using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Spin camera
    /// </summary>
    [PacketDef(SendPacketDefinition.SpinCamera)]
    public sealed class SendSpinCamera : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            throw new NotImplementedException();
        }

    }

}
