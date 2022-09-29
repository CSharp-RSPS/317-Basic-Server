using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util.Annotations;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{

    /// <summary>
    /// This packet is sent when the player attempts to cast magic onto another.
    /// </summary>
    [PacketInfo(249, 4)]
    public sealed class ReceiveMagicOnPlayer : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int playerIndex = reader.ReadShortAdditional();
            int spellId = reader.ReadShortLittleEndian();

            Player? other = WorldHandler.World.Players.ByPlayerIndex(playerIndex);

            if (other == null)
            {
                return;
            }
        }

    }
}
