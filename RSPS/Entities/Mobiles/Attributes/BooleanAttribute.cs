using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Mobiles.Attributes
{
    internal class BooleanAttribute : Attribute<Boolean>
    {
        public BooleanAttribute(bool value) : base(value, AttributeType.BOOLEAN)
        {}

        public override string ToString()
        {
            return Value ? bool.TrueString : bool.FalseString;
        }
    }
}
