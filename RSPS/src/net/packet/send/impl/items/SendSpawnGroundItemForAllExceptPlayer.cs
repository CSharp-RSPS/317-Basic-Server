using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Spawn ground item for all except specified player
    /// </summary>
    public sealed class SendSpawnGroundItemForAllExceptPlayer : ISendPacket
    {


        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter writer = Packet.CreatePacketWriter(8);
            writer.WriteHeader(encryptor, 215);
            return writer;
        }

    }

}
