using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Sets the mini map's state.
    /// </summary>
    [PacketDef(PacketDefinition.MinimapState)]
    public sealed class SendMinimapState : IPacketPayloadBuilder
    {

        /// <summary>
        /// The minimap state
        /// </summary>
        public int State { get; private set; }


        /// <summary>
        /// Creates a new minimap state packet payload builder
        /// </summary>
        /// <param name="state">The minimap state</param>
        public SendMinimapState(int state)
        {
            State = state;
        }   

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(State);
        }

        /*
         * State	Description
0	Active: Clickable and viewable
1	Locked: viewable but not clickable
2	Blacked-out: Minimap is replaced with black background
         * */

    }

}
