using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Shows an interface in the chat box.
    /// </summary>
    [PacketDef(PacketDefinition.ChatInterface)]
    public sealed class SendChatInterface : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }


        /// <summary>
        /// Creates a new chat interface payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        public SendChatInterface(int interfaceId)
        {
            InterfaceId = interfaceId;
        }   

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(InterfaceId, Packet.ByteOrder.LittleEndian);
        }

    }

}
