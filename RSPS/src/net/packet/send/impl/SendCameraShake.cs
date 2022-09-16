using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    public class SendCameraShake : ISendPacket
    {
        public byte[] SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(5);
            writer.WriteHeader(encryptor, 35);
            writer.WriteByte(1);//max of 4 Array Index
            writer.WriteByte(0);//Affects All values
            writer.WriteByte(1);//Y Move Camera
            writer.WriteByte(0);
            return writer.Payload;
        }
    }
}
