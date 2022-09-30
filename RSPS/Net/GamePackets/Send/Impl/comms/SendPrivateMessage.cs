using RSPS.Entities.Mobiles.Players;
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
    [PacketDef(PacketDefinition.SendPrivateMessage)]
    public sealed class SendPrivateMessage : IPacketVariablePayloadBuilder
    {

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
        /// <param name="receiver">The name of the player to be receiving the message, as a long value</param>
        /// <param name="size">The message size</param>
        /// <param name="message">The message</param>
        public SendPrivateMessage(long receiver, int size, byte[] message)
        {
            Receiver = receiver;
            Size = size;
            Message = message;
        }

        public int GetPayloadSize()
        {
            return Size + 1 + 8 + 4; //?
        }

        public void WritePayload(PacketWriter writer)
        {
            /*
            packet.writeHeader(PacketHeader.VARIABLE_BYTE, player.getConnection().getEncryptor(), 196);
            packet.putLong(receiver);
            packet.putInverseInteger(player.getAttributes().getLastPm());
            packet.putByte(player.getRights().getProtocolValue());
            packet.getBuffer().writeBytes(message, 0, size);
            packet.finishPacket(PacketHeader.VARIABLE_BYTE);
            */
        }

        
    }

}
