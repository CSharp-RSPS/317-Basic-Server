using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.Util.Annotations
{
    /// <summary>
    /// Represents an opcode attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class OpcodeAttribute : Attribute
    {

        /// <summary>
        /// The opcode
        /// </summary>
        public int Opcode { get; set; }


        /// <summary>
        /// Creates a new opcode attribute
        /// </summary>
        /// <param name="opcode">The opcode</param>
        public OpcodeAttribute(int opcode)
        {
            Opcode = opcode;
        }

    }
}
