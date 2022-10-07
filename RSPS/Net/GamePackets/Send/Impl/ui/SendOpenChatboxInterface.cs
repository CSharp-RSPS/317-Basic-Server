using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Sending this packet to the client will cause the client to open an interface over the chatbox.
    /// </summary>
    [PacketDef(SendPacketDefinition.OpenChatboxInterface)]
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
            writer.WriteShortAdditionalLittleEndian(InterfaceId);
        }

    }

}
