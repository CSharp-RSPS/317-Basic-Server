using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using RSPS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
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

            Player? other = WorldHandler.World.Players.ByWorldIndex(playerIndex);

            if (other == null)
            {
                return;
            }
        }

    }
}
