using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.movement.Locations
{
    /// <summary>
    /// Represents an area in the game world
    /// </summary>
    public abstract class Area
    {

        /// <summary>
        /// The boundary points of the area
        /// </summary>
        public List<Position> Bounds { get; private set; }
        


        public Area(Position minX, Position maxX, Position minY, Position maxY)
        {
            Bounds = new List<Position>() { minX, maxX, minY, maxY };
        }

        public Area(Position minX, Position minY, Position center)
        {

        }

        public Area(Position center, int radius)
        {

        }

        /// <summary>
        /// Creates a new area
        /// </summary>
        /// <param name="positions">The boundary points of the area</param>
        public Area(List<Position> bounds)
        {
            Bounds = bounds;
        }

        /// <summary>
        /// Retrieves whether a position is within the area
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns>The result</returns>
        public abstract bool InArea(Position position);

        /// <summary>
        /// Retrieves a random position within the area
        /// </summary>
        /// <returns>The position</returns>
        public abstract Position GetRandomPosition();

    }
}
