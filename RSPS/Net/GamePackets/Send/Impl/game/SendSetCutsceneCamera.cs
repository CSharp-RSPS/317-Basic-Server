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
            writer.WriteByte(0); // X offset
            writer.WriteByte(0); // Y offset
            writer.WriteShort(0); // Z offset
            writer.WriteByte(0); // ?
            writer.WriteByte(0); // ?
        }

    }

}
