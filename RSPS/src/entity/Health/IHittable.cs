using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.Health
{
    /// <summary>
    /// Represents an entity that can be hit
    /// </summary>
    public interface IHittable
    {

        /// <summary>
        /// The hitpoints
        /// </summary>
        public Hitpoints Hitpoints { get; set; }

    }
}
