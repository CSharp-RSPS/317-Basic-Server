using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sends a ignored player to the ignore list.
    /// </summary>
    public sealed class SendAddIgnore : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(1);
            writer.WriteVariableShortHeader(encryptor, 214);

            writer.FinishVariableShortHeader();
            return writer;
        }

    }

}
