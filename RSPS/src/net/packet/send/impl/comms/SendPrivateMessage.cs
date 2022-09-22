using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sends a private message to another player.
    /// </summary>
    public sealed class SendPrivateMessage : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(1);
            writer.WriteVariableHeader(encryptor, 196);

            writer.FinishVariableHeader();
            return writer;
        }

    }

}
