using RSPS.src.entity.Mobiles.Npcs;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update.block
{
    public class NPCMask : IUpdateProtocol<Npc>
    {
        public void Process()
        {
            throw new NotImplementedException();
        }

        public void Process(Npc entity, PacketWriter writer, PacketWriter stateBlock)
        {
            throw new NotImplementedException();
        }
    }
}
