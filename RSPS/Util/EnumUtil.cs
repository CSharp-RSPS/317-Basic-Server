using RSPS.Game.Comms.Messaging;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Util
{
    /// <summary>
    /// Contains enum related operations and utilities
    /// </summary>
    public static class EnumUtil
    {


        /// <summary>
        /// Retrieves an attribute of a certain type on an enum value
        /// </summary>
        /// <typeparam name="T">The attribute type</typeparam>
        /// <param name="value">The enum value</param>
        /// <returns>The attribute</returns>
        /// <exception cref="Exception">Thrown when the enum value has no attributes of the requested type</exception>
        public static T GetAttributeOfType<T>(this Enum value) where T : Attribute
        {
            MemberInfo[] memberInfo = value.GetType().GetMember(value.ToString());
            Attribute? attr = memberInfo[0].GetCustomAttribute(typeof(T), false);

            if (attr == null)
            {
                throw new Exception("No attributes found on enum " + value.ToString());
            }
            return (T)attr;
        }

    }
}
