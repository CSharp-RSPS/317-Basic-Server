using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.npc
{
    public class Npc : Entity
    {

        public int Id;
        public int Index;
        public bool Visible;
        public Position Position = new Position(3220, 3220);

        public bool UpdateRequired = false;
        public bool AppearanceUpdateRequired = false;
        public bool ChatUpdateRequired = false;
        public int PrimaryDirection = -1;
        public int SecondaryDirection = -1;

        public Npc(int id)
        {
            Id = id;
            Visible = true;
        }


        public override void ResetFlags()
        {
            UpdateRequired = false;
            AppearanceUpdateRequired = false;
            ChatUpdateRequired = false;
            ResetMovementQueue = false;
            NeedsPlacement = false;
            PrimaryDirection = -1;
            SecondaryDirection = -1;
        }
    }
}
