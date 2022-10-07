using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Mobiles.Attributes
{
    public abstract class Attribute<T>
    {
        protected T Value;
        private AttributeType Type;

        public Attribute(T value, AttributeType type)
        {
            Value = value;
            Type = type;
        }

        public AttributeType GetAttributeType()
        {
            return Type;
        }

        public T GetValue()
        {
            return Value;
        }

        public void SetValue(T value)
        {
            Value = value;
        }

        public abstract override string ToString();

    }
}
