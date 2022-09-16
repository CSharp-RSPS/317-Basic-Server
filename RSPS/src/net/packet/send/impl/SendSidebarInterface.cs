using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    internal class SendSidebarInterface : ISendPacket
    {

        private readonly int InterfaceID;
        private readonly int Slot;

        public SendSidebarInterface(int slot, int interfaceID)
        {
            InterfaceID = interfaceID;
            Slot = slot;
        }

        public byte[] SendPacket(ISAACCipher encryptor)
        {
            PacketWriter packetWriter = Packet.CreatePacketWriter(4);
            packetWriter.WriteHeader(encryptor, 71);
            packetWriter.WriteShort(InterfaceID);
            packetWriter.WriteByte(Slot, Packet.ValueType.A);
            return packetWriter.Payload;
        }
    }
}
