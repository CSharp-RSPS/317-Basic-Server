using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sending this packet to the client will cause the client to open an interface over the chatbox.
    /// </summary>
    [PacketDef(PacketDefinition.OpenChatboxInterface)]
    public sealed class SendOpenChatboxInterface : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }


        /// <summary>
        /// Creates a new open chatbox interface packet payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        public SendOpenChatboxInterface(int interfaceId)
        {
            InterfaceId = interfaceId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(InterfaceId, Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);
        }

    }

}
