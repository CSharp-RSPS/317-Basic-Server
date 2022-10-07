using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Sends the chat privacy settings.
    /// </summary>
    [PacketDef(SendPacketDefinition.ChatSettings)]
    public sealed class SendChatSettings : IPacketPayloadBuilder
    {

        /// <summary>
        /// The public chat setting
        /// </summary>
        public int PublicChat { get; private set; }

        /// <summary>
        /// The private chat setting
        /// </summary>
        public int PrivateChat { get; private set; }

        /// <summary>
        /// The trade setting
        /// </summary>
        public int Trade { get; private set; }


        /// <summary>
        /// Creates a new chat settings payload builder
        /// </summary>
        /// <param name="publicChat">The public chat setting</param>
        /// <param name="privateChat">The private chat setting</param>
        /// <param name="trade">The trade setting</param>
        public SendChatSettings(int publicChat, int privateChat, int trade)
        {
            PublicChat = publicChat;
            PrivateChat = privateChat;
            Trade = trade;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(PublicChat);
            writer.WriteByte(PrivateChat);
            writer.WriteByte(Trade);
        }

    }

}
