using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Play sound in location.
    /// </summary>
    public sealed class SendPlaySoundInLocation : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(5);
            writer.WriteHeader(encryptor, 105);
            return writer;
        }

    }

}
