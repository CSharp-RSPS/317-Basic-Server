using RSPS.Entities.Mobiles.Players;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl.comms
{
    /// <summary>
    /// Sends a clan chat message
    /// </summary>
    [PacketDef(SendPacketDefinition.ClanChatMessage)]
    public sealed class SendClanChatMessage : IPacketPayloadBuilder
    {

        /// <summary>
        /// The username
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// The message
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// The clan name
        /// </summary>
        public string ClanName { get; private set; }

        /// <summary>
        /// The player' rights
        /// </summary>
        public PlayerRights Rights { get; private set; }


        /// <summary>
        /// Sends a clan chat message
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="message">The message</param>
        /// <param name="clanName">The clan name</param>
        /// <param name="rights">The player' rights</param>
        public SendClanChatMessage(string username, string message, string clanName, PlayerRights rights)
        {
            Username = username;
            Message = message;
            ClanName = clanName;
            Rights = rights;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteRS2String(Username);
            writer.WriteRS2String(Message);
            writer.WriteRS2String(ClanName);
            writer.WriteShort((int)Rights);
        }

    }
}
