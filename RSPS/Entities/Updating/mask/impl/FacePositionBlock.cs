using RSPS.Entities.Mobiles;
using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.movement.Locations;
using RSPS.Entities.Updating.block;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating.mask.impl
{
    public class FacePositionBlock : IUpdateMask<Mobile>
    {
        public void AppendBlock(Mobile mobile, PacketWriter updateBlock)
        {
            //Get the facing location
            if (mobile is Player)
            {
                //face location == null... write both 0s
                updateBlock.WriteShortAdditionalLittleEndian(-1);//getX() * 2 + 1
                updateBlock.WriteShortLittleEndian(-1);//getY() * 2 + 1
            }
            else
            {
                //face location == null... write both 0s
                updateBlock.WriteShortLittleEndian(-1);//getX() * 2 + 1
                updateBlock.WriteShortLittleEndian(-1);//getY() * 2 + 1
            }
        }
    }
}
