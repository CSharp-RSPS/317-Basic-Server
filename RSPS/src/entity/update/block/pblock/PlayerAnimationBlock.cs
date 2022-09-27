using RSPS.src.entity.player;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update.block.pblock
{
    internal class PlayerAnimationBlock : IUpdateBlock<Player>
    {
        public void ProcessBlock(Player player, PacketWriter writer)
        {
            writer.WriteShortLittleEndian(-1);
            writer.WriteByteNegated(-1);
        }

    }
}
