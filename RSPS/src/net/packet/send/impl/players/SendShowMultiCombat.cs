using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Shows the player that they are in a multi-combat zone.
    /// </summary>
    public sealed class SendShowMultiCombat : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(2);
            writer.WriteHeader(encryptor, 61);
            return writer;
        }

    }

}
