using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.src.entity.movement.Locations;

namespace RSPS.src.entity.movement
{
    public class MovementPoint : Position
    {

        public int Direction { get; private set; }

        public MovementPoint(int x, int y, int direction) : base(x, y)
        {
            Direction = direction;
        }
    }
}
