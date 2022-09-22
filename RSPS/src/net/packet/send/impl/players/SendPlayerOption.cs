using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Adds a player option to the right click menu of player(s).
    /// </summary>
    public sealed class SendPlayerOption : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(1);
            writer.WriteVariableHeader(encryptor, 104);

            writer.FinishVariableHeader();
            return writer;
        }

    }

}
