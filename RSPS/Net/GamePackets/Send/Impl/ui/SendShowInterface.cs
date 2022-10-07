using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Displays a normal non-walkable interface.
    /// </summary>
    [PacketDef(SendPacketDefinition.ShowInterface)]
    public sealed class SendShowInterface : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }


        /// <summary>
        /// Creates a new show interface packet payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        public SendShowInterface(int interfaceId)
        {
            InterfaceId = interfaceId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(InterfaceId);
        }

    }

}
