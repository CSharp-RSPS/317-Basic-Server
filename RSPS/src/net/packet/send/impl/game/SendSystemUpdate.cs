using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    ///A timer showing how many seconds until a 'System Update' will appear in the lower left hand corner of the game screen. 
    ///After the timer reaches 0 all players are disconnected and are unable to log in again until server is restarted. 
    ///Players connecting will receive a message stating, "The server is being updated. Please wait 1 minute and try again." (unless stated otherwise).
    /// </summary>
    [PacketDef(PacketDefinition.SystemUpdate)]
    public sealed class SendSystemUpdate : IPacketPayloadBuilder
    {

        /// <summary>
        /// The time until the system update
        /// </summary>
        public int TimeUntilUpdate { get; private set; }


        /// <summary>
        /// Creates a new system update packet payload builder
        /// </summary>
        /// <param name="timeUntilUpdate">The time until the system update</param>
        public SendSystemUpdate(int timeUntilUpdate)
        {
            TimeUntilUpdate = timeUntilUpdate;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(TimeUntilUpdate, Packet.ByteOrder.LittleEndian);
        }

    }

}
