using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    public class SendConfiguration : ISendPacket
    {

        public int FrameId;
        public bool FrameState;

        public SendConfiguration(int frameId, bool frameState)
        {
            FrameId = frameId;
            FrameState = frameState;
        }

        public byte[] SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = new PacketWriter(4);
            writer.WriteHeader(encryptor, 36);
            writer.WriteShort(FrameId, Packet.ValueType.STANDARD, Packet.ByteOrder.LITTLE);
            writer.WriteByte(FrameState == true ? 1 : 0);
            return writer.Payload;
        }
    }
}
