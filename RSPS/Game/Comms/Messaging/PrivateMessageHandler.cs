using RSPS.Entities.Mobiles.Players;
using RSPS.Net;
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
    /// Handles private message related operations
    /// </summary>
    public static class PrivateMessageHandler
    {


        /// <summary>
        /// Sends a private message from one to another player
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="receiver">The receiver</param>
        /// <param name="message">The message</param>
        /// <param name="messageSize">The message size</param>
        public static void SendPrivateMessage(Player sender, long receiver, byte[] message, int messageSize)
        {
            string name = Misc.DecodeBase37(receiver);
            Player? other = WorldHandler.World.Players.ByUsername(Misc.DecodeBase37(receiver));

            if (other == null)
            {
                PacketHandler.SendPacket(sender, new SendMessage("This player is not online."));
                return;
            }
           //TODO PacketHandler.SendPacket(other, new SendPrivateMessage(receiver, message, messageSize));
        }

        /// <summary>
        /// Registers a player to the private message handler
        /// </summary>
        /// <param name="player">The player</param>
        public static void Register(Player player)
        {
            PacketHandler.SendPacket(player, new SendFriendsListStatus(2));

            foreach (Player other in WorldHandler.World.Players.Entities.Where(p => p != player))
            { // Mark the player as online for the players that have the player in their friends list

                if (other.Friends.HasContact(other.Credentials.Username))
                {
                    PacketHandler.SendPacket(other, new SendAddFriend(player.Credentials.UsernameAsLong,
                        WorldHandler.World.Details.Id));
                }
            }
            foreach (long usernameAsLong in player.Friends.Usernames)
            { // Mark the contacts of the player that are online as online in the friends list
                Player? other = WorldHandler.World.Players.ByUsername(Misc.DecodeBase37(usernameAsLong));

                PacketHandler.SendPacket(player, new SendAddFriend(usernameAsLong,
                        other == null ? ContactsHandler.ContactOfflineId : WorldHandler.World.Details.Id));
            }
        }

        /// <summary>
        /// Unregisters a player from the private message handler
        /// </summary>
        /// <param name="player">The player</param>
        public static void Unregister(Player player)
        {
            foreach (Player other in WorldHandler.World.Players.Entities.Where(p => p != player))
            { // Mark the player as online for the players that have the player in their friends list

                if (other.Friends.HasContact(other.Credentials.Username))
                {
                    PacketHandler.SendPacket(other, new SendAddFriend(player.Credentials.UsernameAsLong,
                        ContactsHandler.ContactOfflineId));
                }
            }
        }


        //Contacts
        

    }
}
