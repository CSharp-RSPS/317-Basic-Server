namespace RSPS.Entities.Mobiles.Attributes
{
    //Defined Attribute map object in mobile. Line 82 is the define(commented out)

    //DEFINE IF AN ATTRIBUTE IS PERSISTENT
    //FIX GENERIC using sealed type
    public class AttributeMap
    {
        private readonly Dictionary<AttributeName, Attribute<ValueType>> attributeMap = new Dictionary<AttributeName, Attribute<ValueType>>();

        //Can't use generic here because all data types are sealed?
        //Defines the attribute
        public void Define<T>(AttributeName attributeName, Attribute<T> attribute)
        {
            if (attributeMap.ContainsKey(attributeName))
            {
                return;
            }
            //attributeMap.Add(attributeName, attribute);
        }

        //Gets the attribue object
        public Attribute<ValueType>? GetAttribute(AttributeName attributeName)
        {
            if (!attributeMap.ContainsKey(attributeName))
            {
                return null;
            }
            return attributeMap[attributeName];
        }

        //Update the attribute value
        public void Update(AttributeName attributeName, Attribute<ValueType> attribute)
        {
            attributeMap[attributeName] = attribute;
        }

    }
}
