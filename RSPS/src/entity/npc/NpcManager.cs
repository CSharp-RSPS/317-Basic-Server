using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.npc
{
    public class NpcManager : EntityManager<Npc>
    {


        public override Npc Add(Npc npc) {
            base.Add(npc);

            npc.Index = Entities.IndexOf(npc);
            return npc;
        }

        public override void Dispose() {
            GC.SuppressFinalize(this);

            //TODO any logic

            base.Dispose();
        }

    }
}
