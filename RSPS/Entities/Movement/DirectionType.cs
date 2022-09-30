using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.movement
{
    /// <summary>
    /// Holds the possible direction types
    /// </summary>
    public enum DirectionType
    {

        [DirectionValue(-1)]
        None,
        [DirectionValue(0)]
        NorthWest,
        [DirectionValue(1)]
        North,
        [DirectionValue(2)]
        NorthEast,
        [DirectionValue(3)]
        West,
        [DirectionValue(4)]
        East,
        [DirectionValue(5)]
        SouthWest,
        [DirectionValue(6)]
        South,
        [DirectionValue(7)]
        SouthEast

    }
}
