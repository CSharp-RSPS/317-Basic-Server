using RSPS.src.entity.player;
using RSPS.src.net.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Represents a login response packet
    /// </summary>
    public class SendLoginResponse : ISendPacket
    {

        /// <summary>
        /// The login response
        /// </summary>
        public LoginResponse LoginResponse { get; private set; }

        /// <summary>
        /// The player rights
        /// </summary>
        public PlayerRights Rights { get; private set; }

        /// <summary>
        /// Whether the player is flagged
        /// </summary>
        public bool Flagged { get; private set; }


        /// <summary>
        /// Creates a new login response packet
        /// </summary>
        /// <param name="loginResponse">The login response</param>
        public SendLoginResponse(LoginResponse loginResponse, PlayerRights rights = PlayerRights.Default, bool flagged = false)
        {
            LoginResponse = loginResponse;
            Rights = rights;
            Flagged = flagged;
        }

        public byte[] SendPacket(ISAACCipher encryptor)
        {
            PacketWriter pw = Packet.CreatePacketWriter(3);

            pw.WriteByte((int)LoginResponse);
            pw.WriteByte((int)Rights);
            pw.WriteByte(Flagged ? 1 : 0); //1 = flagged (information about mouse movements etc. are sent to the server. Suspected bot accounts are flagged.)

            return pw.Payload;
        }

    }
}
