using RSPS.Entities.Mobiles;
using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.Updating.block;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating.mask.impl
{
    internal class AnimationBlock : IUpdateMask<Mobile>
    {
        public void AppendBlock(Mobile mobile, PacketWriter writer)
        {
            if (mobile is Player)
            {
                writer.WriteShortLittleEndian(-1);//Animation Id
                writer.WriteByteNegated(-1);//delay
            }
            else
            {
                writer.WriteShortLittleEndian(-1);//Animation Id
                writer.WriteByte(-1);//delay
            }
        }

    }
}
