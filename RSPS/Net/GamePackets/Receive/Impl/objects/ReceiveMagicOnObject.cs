using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets.Send.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
{

    /// <summary>
    /// Send when a player uses magic on an object.
    /// </summary>
    public sealed class ReceiveMagicOnObject : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int x = reader.ReadShortLittleEndian();
            int spellId = reader.ReadShortAdditional();
            int y = reader.ReadShortAdditional();
            int objectId = reader.ReadShortLittleEndian();
        }

    }
}
