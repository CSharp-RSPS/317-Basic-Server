using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Assigns an interface to one of the tabs in the game sidebar.
    /// </summary>
    public sealed class SendSidebarInterface : ISendPacket
    {

        public int Slot { get; private set; }

        public int InterfaceID { get; private set; }


        public SendSidebarInterface(int slot, int interfaceID)
        {
            Slot = slot;
            InterfaceID = interfaceID;
        }

        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter packetWriter = Packet.CreatePacketWriter(4);
            packetWriter.WriteHeader(encryptor, 71);
            packetWriter.WriteShort(InterfaceID);
            packetWriter.WriteByte(Slot, Packet.ValueType.Additional);
            return packetWriter;
        }
    }
}
