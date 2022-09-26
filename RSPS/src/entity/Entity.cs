using RSPS.src.entity.player;
using RSPS.src.entity.update.flag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity
{
    public abstract class Entity
    {

        public bool NeedsPlacement = false;

        public bool ResetMovementQueue = false;

        public int WorldIndex = -1;

        public EntityFlags Flags = new EntityFlags();

        public Position CurrentRegion = new Position(0, 0, 0);

        public Entity()
        {

        }

        public abstract void ResetFlags();

    }
}
