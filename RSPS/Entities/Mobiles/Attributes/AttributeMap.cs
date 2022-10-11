using System.Runtime.InteropServices;

namespace RSPS.Entities.Mobiles.Attributes
{
    //Defined Attribute map object in mobile. Line 82 is the define(commented out)

    //DEFINE IF AN ATTRIBUTE IS PERSISTENT - done
    //FIX GENERIC using sealed type
    public class AttributeMap
    {
        private readonly Dictionary<AttributeName, Attribute> attributeMap = new Dictionary<AttributeName, Attribute>(Enum.GetNames(typeof(AttributeName)).Length);
        private readonly List<AttributeName> PersistenceList = new List<AttributeName>(Enum.GetNames(typeof(AttributeName)).Length);

        //Defines the attribute
        public void Define(AttributeName attributeName, Attribute attribute, bool savable = false)
        {
            if (attributeMap.ContainsKey(attributeName))
            {
                return;
            }

            if (savable)
            {
                PersistenceList.Add(attributeName);
            }

            attributeMap.Add(attributeName, attribute);
        }

        //still having problems!
        //Gets the attribue object
        public Attribute<ValueType>? GetAttribute(AttributeName attributeName)
        {
            if (!attributeMap.ContainsKey(attributeName))
            {
                return null;
            }
            return attributeMap[attributeName].getValue;
        }

        //Update the attribute value
        public void Update(AttributeName attributeName, Attribute<ValueType> attribute)
        {
            attributeMap[attributeName] = attribute;
        }

        public Dictionary<AttributeName, Attribute> GetAttributes()
        {
            return attributeMap;
        }

        /**
         * Returns a dictonary of the persistent attributes
         * */
        public Dictionary<AttributeName, Attribute> GetPersistenceAttributes()
        {
            var dict = new Dictionary<AttributeName, Attribute>(PersistenceList.Count);
            foreach (var attribute in PersistenceList)
            {
                dict.Add(attribute, attributeMap[attribute]);
            }
            return dict;
        }

    }
}
