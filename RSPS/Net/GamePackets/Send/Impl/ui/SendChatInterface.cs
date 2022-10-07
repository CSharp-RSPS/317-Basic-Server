using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Shows an interface in the chat box.
    /// </summary>
    [PacketDef(SendPacketDefinition.ChatInterface)]
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
            writer.WriteShortLittleEndian(InterfaceId);
        }

    }

}
