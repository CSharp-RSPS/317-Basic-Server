using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sends a server message (e.g. 'Welcome to RuneScape') or trade/duel request.
    /// </summary>
    [PacketDef(PacketDefinition.SendMessage)]
    public sealed class SendMessage : IPacketVariablePayloadBuilder
    {

        /// <summary>
        /// The message
        /// </summary>
        public string Message { get; private set; }


        /// <summary>
        /// Creates a new message packet
        /// </summary>
        /// <param name="message">The message</param>
        public SendMessage(string message)
        {
            Message = message;
        }

        public int GetPayloadSize()
        {
            return Message.Length;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteString(Message);
        }
    }
}
