﻿using RSPS.src.entity.player;
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

        public Position CurrentRegion = new Position(0, 0, 0);

        public Entity()
        {}

        public abstract void ResetFlags();

    }
}
