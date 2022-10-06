using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using RSPS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Comms.Messaging
{
    /// <summary>
    /// Handles contacts related operations
    /// </summary>
    public static class ContactsHandler
    {

        /// <summary>
        /// The identifier used to indicate a contact is offline
        /// </summary>
        public static readonly int ContactOfflineId = 0;


        /// <summary>
        /// Adds a new friend for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="username">The username as long value</param>
        public static void AddFriend(Player player, long username)
        {
            AddContact(player, ContactType.Friends, username);
        }

        /// <summary>
        /// Adds a new ignore for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="username">The username as long value</param>
        public static void AddIgnore(Player player, long username)
        {
            AddContact(player, ContactType.Ignores, username);
        }

        /// <summary>
        /// Adds a new contact for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="contactType">The type of contact</param>
        /// <param name="username">The username as long value</param>
        public static void AddContact(Player player, ContactType contactType, long username)
        {
            Contacts contacts = contactType == ContactType.Friends ? player.Comms.Friends : player.Comms.Ignores;

            if (player.Credentials.UsernameAsLong == username)
            {
                PacketHandler.SendPacket(player, new SendMessage("You can not add yourself."));
                return;
            }
            if (contacts.HasContact(username))
            {
                PacketHandler.SendPacket(player, new SendMessage("This player is already in your " + contactType.ToString().ToLower() + " list."));
                return;
            }
            Contacts otherContacts = contactType == ContactType.Friends ? player.Comms.Ignores : player.Comms.Friends;

            if (otherContacts.HasContact(username))
            {
                PacketHandler.SendPacket(player, new SendMessage(
                    "This player is already in your " + otherContacts.ContactType.ToString().ToLower() + " list."));
                return;
            }
            //TODO check if username exists in general, so not only online players

            if (!contacts.Add(username))
            {
                PacketHandler.SendPacket(player, new SendMessage(
                    "You can't have more than " + contacts.Limit + " " + contactType.ToString().ToLower() + "."));
                return;
            }
            Player? other = WorldHandler.World.Players.ByUsername(Misc.DecodeBase37(username));
            // TODO chat settings (in case other player needs to appear offline etc)
            PacketHandler.SendPacket(player, contactType == ContactType.Friends
                ? new SendAddFriend(username, other != null && other.LoggedIn ? WorldHandler.World.Details.Id : ContactOfflineId)
                : new SendAddIgnore(player.Comms.Ignores.Usernames));
        }

        /// <summary>
        /// Removes a friend for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="username">The username as long value</param>
        public static void RemoveFriend(Player player, long username)
        {
            RemoveContact(player, ContactType.Friends, username);
        }

        /// <summary>
        /// Removes an ignore for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="username">The username as long value</param>
        public static void RemoveIgnore(Player player, long username)
        {
            RemoveContact(player, ContactType.Ignores, username);
        }

        /// <summary>
        /// Removes a contact for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="contactType">The type of contact</param>
        /// <param name="username">The username as long value</param>
        public static void RemoveContact(Player player, ContactType contactType, long username)
        {
            Contacts contacts = contactType == ContactType.Friends ? player.Comms.Friends : player.Comms.Ignores;

            if (!contacts.HasContact(username))
            {
                PacketHandler.SendPacket(player, new SendMessage("This player is not in your " + contactType.ToString().ToLower() + " list."));
                return;
            }
            contacts.Remove(username);
        }

    }
}
