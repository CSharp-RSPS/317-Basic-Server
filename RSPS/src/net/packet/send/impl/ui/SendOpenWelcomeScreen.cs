using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// This packet displays the welcome screen.
    /// </summary>
    [PacketDef(PacketDefinition.OpenWelcomeScreen)]
    public sealed class SendOpenWelcomeScreen : IPacketPayloadBuilder
    {

        /// <summary>
        /// Whether the player is a member
        /// </summary>
        public bool Member { get; private set; }

        /// <summary>
        /// The days since the last recovery change
        /// </summary>
        public int DaysSinceLastRecoveryChange { get; private set; }

        /// <summary>
        /// The amount of unread messages
        /// </summary>
        public int UnreadMessageCount { get; private set; }

        /// <summary>
        /// The IP the player last logged in from
        /// </summary>
        public int LastLoggedIp { get; private set; }

        /// <summary>
        /// The last successful login
        /// </summary>
        public int LastSuccessfulLogin { get; private set; }


        /// <summary>
        /// Creates a new open welcome screen packet payload builder
        /// </summary>
        /// <param name="member">Whether the player is a member</param>
        /// <param name="daysSinceLastRecoveryChange">The days since the last recovery change</param>
        /// <param name="unreadMessagCount">The amount of unread messages</param>
        /// <param name="lastLoggedIp">The IP the player last logged in from</param>
        /// <param name="lastSuccessfulLogin">The last successful login</param>
        public SendOpenWelcomeScreen(bool member, int daysSinceLastRecoveryChange, int unreadMessagCount, 
            int lastLoggedIp, int lastSuccessfulLogin)
        {
            Member = member;
            DaysSinceLastRecoveryChange = daysSinceLastRecoveryChange;
            UnreadMessageCount = unreadMessagCount;
            LastLoggedIp = lastLoggedIp;
            LastSuccessfulLogin = lastSuccessfulLogin;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByteNegated(DaysSinceLastRecoveryChange);
            writer.WriteShortAdditional(UnreadMessageCount);
            writer.WriteByte(Member);
            writer.WriteIntMiddleEndian(LastLoggedIp); // middle endian reverse?
            writer.WriteShort(LastSuccessfulLogin);
        }

    }

}
