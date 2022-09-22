using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Forcefully changes a client's setting's value. Also changes the default value for that setting.
    /// </summary>
    public sealed class SendForceClientSetting : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(4);
            writer.WriteHeader(encryptor, 36);
            return writer;
        }

    }

}
