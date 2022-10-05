using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Util.Attributes
{
    /// <summary>
    /// Represents a facial expression attribute
    /// </summary>
    public sealed class FacialExpressionAttribute : Attribute
    {

        /// <summary>
        /// The expression value
        /// </summary>
        public int Value { get; private set; }


        /// <summary>
        /// Creates a new facial expression attribute
        /// </summary>
        /// <param name="value">The expression value</param>
        public FacialExpressionAttribute(int value)
        {
            Value = value;
        }
    }
}
