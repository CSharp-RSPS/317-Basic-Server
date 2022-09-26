using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.movement
{
    /// <summary>
    /// Contains movement related utilities
    /// </summary>
    public static class MovementUtil
    {


        /// <summary>
        /// Retrieves the protocol direction value for a direction
        /// </summary>
        /// <param name="dir">The direction</param>
        /// <returns>The value</returns>
        public static int GetDirectionValue(this DirectionType dir)
        {
            FieldInfo? fieldInfo = dir.GetType().GetField(dir.ToString());

            if (fieldInfo == null)
            {
                return -1;
            }
            return fieldInfo.GetCustomAttributes(typeof(DirectionValueAttribute), false).FirstOrDefault() 
                is not DirectionValueAttribute dirValueAttr ? -1 : dirValueAttr.Value;
        }

        /// <summary>
        /// Retrieves the direction for given delta's.
        /// </summary>
        /// <param name="deltaX">The delta X coordinate</param>
        /// <param name="deltaY">The delta Y coordinate</param>
        /// <returns>The direction</returns>
        public static DirectionType GetDirection(int deltaX, int deltaY)
        {
            if (deltaX < 0)
            {
                if (deltaY < 0)
                {
                    return DirectionType.SouthWest;
                }
                if (deltaY > 0)
                {
                    return DirectionType.NorthWest;
                }
                return DirectionType.West;
            }
            if (deltaX > 0)
            {
                if (deltaY < 0)
                {
                    return DirectionType.SouthEast;
                }
                if (deltaY > 0)
                {
                    return DirectionType.NorthEast;
                }
                return DirectionType.East;
            }
            if (deltaY < 0)
            {
                return DirectionType.South;
            }
            if (deltaY > 0)
            {
                return DirectionType.North;
            }
            return DirectionType.None;
        }

    }
}
