using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    [PacketDef(PacketDefinition.ForceClientSetting)]
    public sealed class SendConfiguration : IPacketPayloadBuilder
    {

        public int FrameId { get; private set; }
        public bool FrameState { get; private set; }

        public SendConfiguration(int frameId, bool frameState)
        {
            FrameId = frameId;
            FrameState = frameState;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(FrameId, Packet.ValueType.Standard, Packet.ByteOrder.LittleEndian);
            writer.WriteByte(FrameState == true ? 1 : 0);
        }
    }
}
