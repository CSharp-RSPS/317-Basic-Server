using RSPS.Util.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.movement.Locations
{
    /// <summary>
    /// Represents an area in the game world
    /// </summary>
    public class Area
    {

        /// <summary>
        /// The grid points of the area
        /// </summary>
        public List<Point2D> Grid { get; private set; }

        /// <summary>
        /// The Z-axis value
        /// </summary>
        public int Z { get; private set; }
        

        /// <summary>
        /// Creates a new area based on a given grid of points
        /// </summary>
        /// <param name="grid">The grid points of the area</param>
        /// <param name="z">The Z-axis value</param>
        protected Area(List<Point2D> grid, int z = 0)
        {
            Grid = grid;
            Z = z;
        }

        /// <summary>
        /// Builds a rectangular grid
        /// </summary>
        /// <param name="minX">The min. X coordinate</param>
        /// <param name="maxX">The max. X coordinate</param>
        /// <param name="minY">The min. Y coordinate</param>
        /// <param name="maxY">The max. Y coordinate</param>
        /// <param name="z">The Z coordinate</param>
        /// <returns>The area</returns>
        public static Area? BuildRectangularGrid(int minX, int maxX, int minY, int maxY, int z = 0)
        {
            if (minX > maxX || minY > maxY)
            {
                return null;
            }
            List<Point2D> grid = new();

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    grid.Add(new Point2D(x, y));
                }
            }
            return new Area(grid, z);
        }

        /// <summary>
        /// Builds an area grid based of specified boundary points
        /// </summary>
        /// <param name="boundaryPoints">The boundary points</param>
        /// <param name="z">The Z-axis value</param>
        /// <returns>The area when successfuly built</returns>
        public static Area? BuildFreeFormGrid(List<Point2D> boundaryPoints, int z = 0)
        { // TODO: recording command that records the points we're walking to form the boundaries until we stop recording
            List<Point2D> grid = new(); // Built as ↑→

            int minY = boundaryPoints.Min(p => p.Y); // The min. Y coordinate from the boundary points
            int maxY = boundaryPoints.Max(p => p.Y); // The max. Y coordinate from the boundary points
            
            for (int y = minY; y <= maxY; y++)
            { // Start at min. Y and loop to max. Y ↑
                List<Point2D> xPoints = boundaryPoints.Where(p => p.Y == y)
                    .OrderBy(p => p.X).ToList(); // Select the x-axis boundary points on the selected Y axis and order them
                
                if (!xPoints.Any())
                { // No boundary points found on the X-axis for the selected Y-axis
                    continue;
                }
                for (int i = 0; i < xPoints.Count; i++)
                { // Find the axis points between boundary points to populate the grid
                    if (i == xPoints.Count - 1)
                    { // We reached the last boundary point, add it to the grid
                        grid.Add(xPoints[i]);
                        break;
                    }
                    Point2D boundaryPoint = xPoints[i]; // The currently selected boundary point
                    grid.Add(boundaryPoint); // Add the boundary point to the grid

                    for (int x = boundaryPoint.X + 1; x < xPoints[i + 1].X; x++)
                    { // Loop the points between the selected and the next boundary point on the axis
                        Point2D point = new(x, y);

                        bool hasBoundaryPointAbove = boundaryPoints.FirstOrDefault(bp => bp.X == x && bp.Y > y) != null;
                        bool hasBoundaryPointBelow = boundaryPoints.FirstOrDefault(bp => bp.X == x && bp.Y < y) != null;

                        if (!hasBoundaryPointAbove || !hasBoundaryPointBelow)
                        { // Point is outside the bounds of the area grid
                            continue;
                        }
                        grid.Add(new(x, y));
                    }
                }
            }
            return new Area(grid, z);
        }

        /// <summary>
        /// Retrieves whether a position is within the area
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns>The result</returns>
        public bool InArea(Position position)
        {
            return InArea(new Point2D(position.X, position.Y), position.Z);
        }

        /// <summary>
        /// Retrieves whether a point is within the area
        /// </summary>
        /// <param name="point">The point</param>
        /// <param name="z">The Z coordinate</param>
        /// <returns>The result</returns>
        public bool InArea(Point2D point, int z)
        {
            return Z == z && Grid.FirstOrDefault(gp => gp.Equals(point)) != null;
        }

        /// <summary>
        /// Retrieves a random point in the area grid
        /// </summary>
        /// <returns>The random point</returns>
        public Point2D GetRandomPoint()
        {
            return Grid[RandomUtil.RandomInt(Grid.Count - 1)];
        }

        /// <summary>
        /// Retrieves a random position within the area
        /// </summary>
        /// <returns>The position</returns>
        public Position GetRandomPosition()
        {
            Point2D point = GetRandomPoint();
            return new Position(point.X, point.Y, Z);
        }

    }
}
