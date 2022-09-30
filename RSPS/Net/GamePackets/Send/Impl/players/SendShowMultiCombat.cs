using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Sending this packet to the client will make the client show the player if they are in a multi-combat zone.
    /// </summary>
    [PacketDef(PacketDefinition.ShowMultiCombat)]
    public sealed class SendShowMultiCombat : IPacketPayloadBuilder
    {

        /// <summary>
        /// Whether to show the multi combat icon
        /// </summary>
        public bool State { get; private set; }


        /// <summary>
        /// Creates a new show multi combat packet payload builder
        /// </summary>
        /// <param name="state">Whether to show the multi combat icon</param>
        public SendShowMultiCombat(bool state)
        {
            State = state;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(State);
        }

        /*
         * ID	Name
0	Not in a multi-combat zone (i.e. no crossbones in bottom-right).
1	In a multi-combat zone (i.e. crossbones in bottom-right).
         * */

    }

}
