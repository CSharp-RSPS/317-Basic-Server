using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.movement.Locations
{
    /// <summary>
    /// Represents a 2D point
    /// </summary>
    public sealed class Point2D
    {

        /// <summary>
        /// The point on the X axis
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The point on the Y axis
        /// </summary>
        public int Y { get; set; }


        /// <summary>
        /// Creates a new 2D point
        /// </summary>
        /// <param name="x">The point on the X axis</param>
        /// <param name="y">The point on the Y axis</param>
        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Retrieves whether a given point matches ours
        /// </summary>
        /// <param name="other">The other point</param>
        /// <returns>The result</returns>
        public bool Equals(Point2D other)
        {
            return other.X == X && other.Y == Y;
        }

    }
}
