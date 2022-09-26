using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.movement.Locations
{
    /// <summary>
    /// Represents a position in the game world
    /// </summary>
    public class Position
    {

        /// <summary>
        /// The X coordinate
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// The Y coordinate
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// The Z coordinate
        /// </summary>
        public int Z { get; set; }

        /// <summary>
        /// The region X coordinate
        /// </summary>
        public int RegionX => (X >> 3) - 6;

        /// <summary>
        /// The region Y coordinate
        /// </summary>
        public int RegionY => (Y >> 3) - 6;

        /// <summary>
        /// The local X coordinate
        /// </summary>
        public int LocalX => X - 8 * RegionX;

        /// <summary>
        /// The local Y coordinate
        /// </summary>
        public int LocalY => Y - 8 * RegionY;


        /// <summary>
        /// Creates a new position from another position
        /// </summary>
        /// <param name="position">The other position</param>
        public Position(Position position) : this(position.X, position.Y, position.Z) { }

        /// <summary>
        /// Creates a new position in the game world
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="z">The Z coordinate</param>
        public Position(int x, int y, int z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Sets the position to another position
        /// </summary>
        /// <param name="other">The other position</param>
        public void SetNewPosition(Position other)
        {
            X = other.X;
            Y = other.Y;
            Z = other.Z;
        }

        /// <summary>
        /// Moves a position by given offsets
        /// </summary>
        /// <param name="offsetX">The X offset</param>
        /// <param name="offsetY">The Y offset</param>
        /// <param name="offsetZ">The Z offset</param>
        public void MovePosition(int offsetX, int offsetY, int offsetZ = 0)
        {
            X += offsetX;
            Y += offsetY;
            Z += offsetZ;
        }

        /// <summary>
        /// Retrieves the local X coordinate based on a given position
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns>The local X coordinate</returns>
        public int GetLocalX(Position position)
        {
            return X - 8 * position.RegionX;
        }

        /// <summary>
        /// Retrieves the local Y coordinate based on a given position
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns>The local Y coordinate</returns>
        public int GetLocalY(Position position)
        {
            return Y - 8 * position.RegionY;
        }

        /**
         * Returns the delta coordinates. Note that the returned Position is not an
         * actual position, instead it's values represent the delta values between
         * the two arguments.
         *
         * @param a the first position
         * @param b the second position
         * @return the delta coordinates contained within a position
         */
        public static Position Delta(Position a, Position b)
        {
            return new Position(b.X - a.X, b.Y - a.Y);
        }

        /// <summary>
        /// Retrieves the distance to a given position
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns>The distance</returns>
        public int GetDistance(Position position)
        {
            Position delta = Delta(this, position);
            return delta.X < delta.Y ? delta.X : delta.Y;
        }

        /// <summary>
        /// Retrieves whether a position is within distance from another position using the default viewing distance of 15 tiles
        /// </summary>
        /// <param name="other">The other position</param>
        /// <returns>The result</returns>
        public bool IsWithinDistance(Position other)
        {
            return IsWithinDistance(other, 15);
        }

        /// <summary>
        /// Retrieves whether a given position is within a specific distance of our position.
        /// </summary>
        /// <param name="other">The other position</param>
        /// <param name="maxDistance">The max. distance to be considered within distance</param>
        /// <param name="ignoreZ">Whether to ignore the Z axis</param>
        /// <returns>The result</returns>
        public bool IsWithinDistance(Position other, int maxDistance, bool ignoreZ = false)
        {
            if (Z != other.Z && !ignoreZ)
            {
                return false;
            }
            Position delta = Delta(this, other);

            return delta.X <= maxDistance - 1 && delta.X >= -maxDistance
                && delta.Y <= maxDistance - 1 && delta.Y >= -maxDistance;
        }

        /// <summary>
        /// Copies the position
        /// </summary>
        /// <returns>The copied position</returns>
        public Position Copy()
        {
            return new Position(X, Y, Z);
        }

        /// <summary>
        /// Retrieves whether given coordinates match the position
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        /// <param name="z">The Z coordinate</param>
        /// <returns>The result</returns>
        public bool Equals(int x, int y, int z)
        {
            return X == x && Y == y && Z == z;
        }

        /// <summary>
        /// Retrieves whether a given position match the position
        /// </summary>
        /// <param name="other">The other position</param>
        /// <returns>The result</returns>
        public bool Equals(Position other)
        {
            return Equals(other.X, other.Y, other.Z);
        }

        public override string ToString()
        {
            return "Position(" + X + ", " + Y + ", " + Z + ")";
        }

    }
}
