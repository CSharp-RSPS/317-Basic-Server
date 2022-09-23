using RSPS.src.entity.player;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{

    /// <summary>
    /// Sent when a player uses an item on another player.
    /// </summary>
    [Opcode(14)]
    public sealed class ReceiveItemOnPlayer : IReceivePacket
    {


        public void ReceivePacket(Player player, int packetOpcode, int packetSize, PacketReader packetReader)
        {
            int value1 = packetReader.ReadShort(Packet.ValueType.Additional);
            int value2 = packetReader.ReadShort(Packet.ByteOrder.BigEndian);
            int value3 = packetReader.ReadShort(Packet.ByteOrder.BigEndian);
            int value4 = packetReader.ReadShort(Packet.ByteOrder.LittleEndian);
        }

    }
}
