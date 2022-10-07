using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Comms.Messaging;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Sends a private message to another player.
    /// </summary>
    [PacketDef(SendPacketDefinition.SendPrivateMessage)]
    public sealed class SendPrivateMessage : IPacketVariablePayloadBuilder
    {

        /// <summary>
        /// The communication handler
        /// </summary>
        public Communication Comms { get; private set; }

        /// <summary>
        /// The rights of the player
        /// </summary>
        public PlayerRights Rights { get; private set; }

        /// <summary>
        /// The name of the player to be receiving the message, as a long value
        /// </summary>
        public long Receiver { get; private set; }

        /// <summary>
        /// The message size
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// The message
        /// </summary>
        public byte[] Message { get; private set; }


        /// <summary>
        /// Creates a new private message packet payload builder
        /// </summary>
        /// <param name="comms">The communication handler</param>
        /// <param name="rights">The rights of the player</param>
        /// <param name="receiver">The name of the player to be receiving the message, as a long value</param>
        /// <param name="size">The message size</param>
        /// <param name="message">The message</param>
        public SendPrivateMessage(Communication comms, PlayerRights rights, long receiver, int size, byte[] message)
        {
            Comms = comms;
            Rights = rights;
            Receiver = receiver;
            Size = size;
            Message = message;
        }

        public int GetPayloadSize()
        {
            return 8 + 4 + 1 + Size;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteLong(Receiver);
            writer.WriteShort(Comms.LastPrivateMessageId);
            writer.WriteByte((int)Rights);
            writer.WriteBytes(Message, Size); //packet.getBuffer().writeBytes(message, 0, size);
        }

        
    }

}
