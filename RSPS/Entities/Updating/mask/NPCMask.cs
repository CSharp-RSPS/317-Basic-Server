using RSPS.Entities.Mobiles.Npcs;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating.block
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
