using RSPS.Entities.Mobiles;
using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating
{
    public class EntityUpdate<T> where T : Mobile
    {
        protected T? Mobile;
        protected PacketWriter Packet;
        protected PacketWriter UpdateBlock;

        public EntityUpdate(T t, PacketWriter packet, PacketWriter updateBlock) {
            Mobile = t;
            Packet = packet;
            UpdateBlock = updateBlock;
        }

    }
}
