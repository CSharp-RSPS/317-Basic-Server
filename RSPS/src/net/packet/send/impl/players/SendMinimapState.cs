using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sets the mini map's state.
    /// </summary>
    public sealed class SendMinimapState : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(2);
            writer.WriteHeader(encryptor, 99);
            return writer;
        }

    }

}
