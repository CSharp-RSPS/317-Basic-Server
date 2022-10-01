using RSPS.Net.GamePackets.Send;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Comms.Messaging
{
    /// <summary>
    /// Holds players contacts
    /// </summary>
    public sealed class Contacts
    {

        /// <summary>
        /// The type of contacts
        /// </summary>
        public ContactType ContactType { get; private set; }

        /// <summary>
        /// The limit of contacts
        /// </summary>
        public int Limit { get; private set; }

        /// <summary>
        /// The usernames as long values
        /// </summary>
        private readonly List<long> usernames;

        /// <summary>
        /// The readonly usernames collection for exposing the usernames
        /// </summary>
        public ReadOnlyCollection<long> Usernames { get; private set; }

        /// <summary>
        /// The current amount of contacts
        /// </summary>
        public int Count => usernames.Count;


        /// <summary>
        /// Creates a new contacts container
        /// </summary>
        /// <param name="contactType">The type of contacts</param>
        public Contacts(ContactType contactType)
        {
            ContactType = contactType;
            Limit = GetContactsLimit(contactType);
            usernames = new(Limit);
            Usernames = usernames.AsReadOnly();
        }

        /// <summary>
        /// Whether a contact with a given username is present
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>The result</returns>
        public bool HasContact(string username)
        {
            return HasContact(Misc.EncodeBase37(username));
        }

        /// <summary>
        /// Whether a contact with a given username as long value is present
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>The result</returns>
        public bool HasContact(long username)
        {
            return usernames.Contains(username);
        }

        /// <summary>
        /// Attempts to add a contact
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>Whether adding was succesful</returns>
        public bool Add(string username)
        {
            return Add(Misc.EncodeBase37(username));
        }

        /// <summary>
        /// Attempts to add a contact by it's long value username
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>Whether adding was succesful</returns>
        public bool Add(long username)
        {
            if (Count >= Limit)
            {
                return false;
            }
            usernames.Add(username);
            return true;
        }

        /// <summary>
        /// Removes a contact
        /// </summary>
        /// <param name="username"></param>
        public void Remove(string username)
        {
            Remove(Misc.EncodeBase37(username));
        }

        /// <summary>
        /// Removes a contact by it's long value username
        /// </summary>
        /// <param name="username"></param>
        public void Remove(long username)
        {
            usernames.Remove(username);
        }

        /// <summary>
        /// Retrieves the contacts limit for a contact type
        /// </summary>
        /// <param name="contactType">The contact type value</param>
        /// <returns>The contacts limit</returns>
        private static int GetContactsLimit(ContactType contactType)
        {
            FieldInfo? fieldInfo = contactType.GetType().GetField(contactType.ToString());

            if (fieldInfo == null)
            {
                return 0;
            }
            return fieldInfo.GetCustomAttributes(typeof(ContactLimitAttribute), false).FirstOrDefault() is not ContactLimitAttribute attr ? 0 : attr.Max;
        }

    }
}
