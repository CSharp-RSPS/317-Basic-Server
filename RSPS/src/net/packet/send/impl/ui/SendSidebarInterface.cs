using RSPS.src.Util.Annotations;
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
    [PacketDef(PacketDefinition.SendSidebarInterface)]
    public sealed class SendSidebarInterface : IPacketPayloadBuilder
    {

        public int Slot { get; private set; }

        public int InterfaceID { get; private set; }


        public SendSidebarInterface(int slot, int interfaceID)
        {
            Slot = slot;
            InterfaceID = interfaceID;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(InterfaceID);
            writer.WriteByte(Slot, Packet.ValueType.Additional);
        }
    }
}
