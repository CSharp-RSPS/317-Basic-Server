using RSPS.src.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    public sealed class ReceiveEquipItem : IReceivePacket
    {


        public void ReceivePacket(Player player, int packetOpCode, int packetLength, PacketReader packetReader)
        {
            int id = packetReader.ReadShort(false);
            int slot = packetReader.ReadShort(false, Packet.ValueType.A);
            int interfaceId = packetReader.ReadShort(false, Packet.ValueType.A);

            if (id < 0 || slot < 0 || interfaceId < 0)
            {
                return;
            }
        }

    }
}
