using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send
{
    public interface ISendPacket
    {

        public PacketWriter SendPacket(ISAACCipher encryptor);

    }
}
