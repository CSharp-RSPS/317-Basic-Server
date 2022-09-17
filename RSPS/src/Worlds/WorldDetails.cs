using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.Worlds
{
    /// <summary>
    /// Holds details about a world
    /// </summary>
    public class WorldDetails
    {

        /// <summary>
        /// The world identifier
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The name of the world
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Whether deep debugging is enabled for the world
        /// </summary>
        public bool Debugging { get; set; }


        /// <summary>
        /// Creates new world details
        /// </summary>
        /// <param name="id">The world identifier</param>
        /// <param name="name">The name of the world</param>
        /// <param name="debugging">Whether deep debugging is enabled for the world</param>
        public WorldDetails(int id, string name, bool debugging = false)
        {
            Id = id;
            Name = name;
            Debugging = debugging;
        }
    }
}
