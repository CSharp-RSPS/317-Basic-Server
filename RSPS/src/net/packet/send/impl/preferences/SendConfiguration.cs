using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    public sealed class SendConfiguration : ISendPacket
    {

        public int FrameId { get; private set; }
        public bool FrameState { get; private set; }

        public SendConfiguration(int frameId, bool frameState)
        {
            FrameId = frameId;
            FrameState = frameState;
        }

        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = new PacketWriter(4);
            writer.WriteHeader(encryptor, 36);
            writer.WriteShort(FrameId, Packet.ValueType.Standard, Packet.ByteOrder.LittleEndian);
            writer.WriteByte(FrameState == true ? 1 : 0);
            return writer;
        }
    }
}
