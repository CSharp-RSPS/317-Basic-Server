using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Begins the player update procedure
    /// </summary>
    public sealed class SendBeginPlayerUpdating : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(1);
            writer.WriteVariableHeader(encryptor, 81);

            writer.FinishVariableHeader();
            return writer;
        }

    }

}
