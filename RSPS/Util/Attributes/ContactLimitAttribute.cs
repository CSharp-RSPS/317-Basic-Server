using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Util.Attributes
{
    /// <summary>
    /// Represents a contacts limit attribute
    /// </summary>
    public sealed class ContactLimitAttribute : Attribute
    {

        /// <summary>
        /// The max. amount of contacts possible
        /// </summary>
        public int Max { get; private set; }


        /// <summary>
        /// Creates a new contacts limit attribute
        /// </summary>
        /// <param name="max">The max. amount of contacts possible</param>
        public ContactLimitAttribute(int max)
        {
            Max = max;
        }

    }
}
