using RSPS.Entities.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities
{
    /// <summary>
    /// Represents an entity that can engage in combat situations
    /// </summary>
    public interface ICombatable : IHittable
    {

        public dynamic Combat { get; set; }

        public dynamic FightType { get; set; }

        public bool AutoRetialate { get; set; }

    }
}
