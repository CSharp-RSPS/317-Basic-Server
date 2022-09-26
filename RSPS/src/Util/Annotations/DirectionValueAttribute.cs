using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.Util.Annotations
{
    /// <summary>
    /// Represents a direction value attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DirectionValueAttribute : Attribute
    {

        /// <summary>
        /// The value
        /// </summary>
        public int Value { get; private set; }


        /// <summary>
        /// Creates a new direction value attribute
        /// </summary>
        /// <param name="value">The value</param>
        public DirectionValueAttribute(int value)
        {
            Value = value;
        }

    }
}
