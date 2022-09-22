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
    public sealed class SendForceClientSetting2 : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(8);
            writer.WriteHeader(encryptor, 87);
            return writer;
        }

    }

}
