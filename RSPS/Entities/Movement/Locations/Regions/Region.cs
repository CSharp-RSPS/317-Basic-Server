using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.movement.Locations.Regions
{
    /// <summary>
    /// Represents a map region in the game world
    /// </summary>
    public sealed class Region
    {

        /// <summary>
        /// The region ID
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The position of the region
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// The clipped tiles within the region
        /// </summary>
        public int[][][] ClippedTiles { get; private set; }


        /// <summary>
        /// Creates a new region
        /// </summary>
        /// <param name="id">The region ID</param>
        /// <param name="position">The position of the region</param>
        public Region(int id, Position position)
        {
            Id = id;
            Position = position;
            ClippedTiles = new int[4][][];
        }

        /// <summary>
        /// Adds a clip for a given position
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="shift">The shift value</param>
        /// <returns>The region</returns>
        public Region AddClip(Position position, int shift)
        {
            int regionAbsX = (Id >> 8) * 64;
            int regionAbsY = (Id & 0xff) * 64;

            if (ClippedTiles[position.Z] == null)
            {
                ClippedTiles[position.Z] = new int[64][];

                for (int i = 0; i < 64; i++)
                {
                    ClippedTiles[position.Z][i] = new int[64];
                }
            }
            ClippedTiles[position.Z][position.X - regionAbsX][position.Y - regionAbsY] |= shift;
            return this;
        }

        /// <summary>
        /// Retrieves the clipping value for a given position
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns>The value</returns>
        public int GetClip(Position position)
        {
            int regionAbsX = (Id >> 8) * 64;
            int regionAbsY = (Id & 0xff) * 64;

            if (ClippedTiles[position.Z] == null)
            {
                return 0;
            }
            return ClippedTiles[position.Z][position.X - regionAbsX][position.Y - regionAbsY];
        }

        /// <summary>
        /// Removes a clipping tile for a given position
        /// </summary>
        /// <param name="position">The position</param>
        /// <returns>The region</returns>
        public Region RemoveClip(Position position)
        {
            int regionAbsX = (Id >> 8) * 64;
            int regionAbsY = (Id & 0xff) * 64;
            int z = position.Z;

            if (z > 4)
            {
                z -= (4 * (z / 4));
            }
            if (z == 4)
            {
                z = 0;
            }
            if (ClippedTiles[z] == null)
            {
                ClippedTiles[z] = new int[64][];

                for (int i = 0; i < 64; i++)
                {
                    ClippedTiles[z][i] = new int[64];
                }
            }
            ClippedTiles[z][position.X - regionAbsX][position.Y - regionAbsY] = 0;
            return this;
        }

    }
}
