using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.src.entity.movement.Locations;

namespace RSPS.src.entity.movement
{
    /// <summary>
    /// Represents a movement point
    /// </summary>
    public sealed class MovementPoint : Position
    {

        /// <summary>
        /// The direction we're moving in
        /// </summary>
        public DirectionType Direction { get; private set; }


        /// <summary>
        /// Creates a new movement point
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="direction">The direction we're moving in</param>
        public MovementPoint(int x, int y, DirectionType direction) : base(x, y)
        {
            Direction = direction;
        }
    }
}
