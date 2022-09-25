using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Sends a private message to another player.
    /// </summary>
    [PacketDef(PacketDefinition.SendPrivateMessage)]
    public sealed class SendPrivateMessage : IPacketVariablePayloadBuilder
    {

        /// <summary>
        /// The player name as long
        /// </summary>
        public long PlayerName { get; private set; }

        /// <summary>
        /// The global message counter
        /// </summary>
        public int GlobalMessageCounter { get; private set; }

        /// <summary>
        /// The player rights
        /// </summary>
        public int Rights { get; private set; }


        /// <summary>
        /// Creates a new private message packet payload builder
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="globalMessageCounter"></param>
        /// <param name="rights"></param>
        public SendPrivateMessage(long playerName, int globalMessageCounter, int rights)
        {
            PlayerName = playerName;
            GlobalMessageCounter = globalMessageCounter;
            Rights = rights;
        }

        public int GetPayloadSize()
        {
            throw new NotImplementedException();
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteLong(PlayerName);
            writer.WriteInt(GlobalMessageCounter);
            writer.WriteByte(Rights);
        }

        
    }

}
