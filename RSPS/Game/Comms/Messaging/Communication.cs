using RSPS.Game.Comms.Chatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Comms.Messaging
{
    /// <summary>
    /// Represents a communication handler for a player
    /// </summary>
    public sealed class Communication
    {

        /// <summary>
        /// The friends
        /// </summary>
        public Contacts Friends { get; private set; } = new (ContactType.Friends);

        /// <summary>
        /// The ignores
        /// </summary>
        public Contacts Ignores { get; private set; } = new(ContactType.Ignores);

        /// <summary>
        /// The identifier of the last private message sent
        /// </summary>
        public int LastPrivateMessageId { get; private set; }

        /// <summary>
        /// The last chat message to be dispatched if any
        /// </summary>
        public ChatMessage? ChatMessage { get; set; }

    }
}
