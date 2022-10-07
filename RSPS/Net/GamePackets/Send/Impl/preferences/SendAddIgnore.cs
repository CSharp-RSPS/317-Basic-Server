using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Sends the IDs of all the users that this player has in their ignore.
    /// Note: By looking at the rest of the 317 protocol, 
    /// there doesn't seem to be a way to change the list dynamically. 
    /// It seems as though that whenever the player decides to add or remove a player from their list, it must send all the values again.
    /// </summary>
    [PacketDef(SendPacketDefinition.SendAddIgnore)]
    public sealed class SendAddIgnore : IPacketVariablePayloadBuilder
    {

        /// <summary>
        /// The usernames as long values
        /// </summary>
        public ICollection<long> UsernamesAsLong { get; private set; }


        /// <summary>
        /// Creates a new add ignore packet payload builder
        /// </summary>
        /// <param name="usernamesAsLong">The usernames as long values</param>
        public SendAddIgnore(ICollection<long> usernamesAsLong)
        {
            UsernamesAsLong = usernamesAsLong;
        }

        public int GetPayloadSize()
        {
            return UsernamesAsLong.Count * 8;
        }

        public void WritePayload(PacketWriter writer)
        {
            foreach (long username in UsernamesAsLong)
            {
                writer.WriteLong(username);
            }
        }

    }

}
