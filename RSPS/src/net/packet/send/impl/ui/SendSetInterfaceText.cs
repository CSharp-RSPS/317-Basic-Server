using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sets the text for the specific interface.
    /// </summary>
    [PacketDef(PacketDefinition.SetInterfaceText)]
    public sealed class SendSetInterfaceText : IPacketVariablePayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// The new text for the interface
        /// </summary>
        public string Text { get; private set; }


        /// <summary>
        /// Creates a new set interface text packet payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        /// <param name="text">The new text for the interface</param>
        public SendSetInterfaceText(int interfaceId, string text)
        {
            InterfaceId = interfaceId;
            Text = text;
        }

        public int GetPayloadSize()
        {
            return Text.Length + 2;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteRS2String(Text)
                .WriteShortAdditional(InterfaceId);
        }

    }

}
