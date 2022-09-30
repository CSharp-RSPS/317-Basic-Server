using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Sends a server side message (e.g. 'Welcome to RuneScape'), or a trade/duel/challenge request.
    /// The format for sending such requests is: [player name][request type]. Where [request type] is one of :duelreq:, :chalreq:, or :tradereq:
    /// Example: Trading a player called 'mopar': mopar:tradereq:
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
            writer.WriteRS2String(Message);
        }

    }
}
