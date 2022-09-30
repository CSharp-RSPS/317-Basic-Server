using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating.block.pblock
{
    internal class PlayerAnimationBlock : IUpdateMask<Player>
    {
        public void ProcessBlock(Player player, PacketWriter writer)
        {
            writer.WriteShortLittleEndian(-1);
            writer.WriteByteNegated(-1);
        }

    }
}
