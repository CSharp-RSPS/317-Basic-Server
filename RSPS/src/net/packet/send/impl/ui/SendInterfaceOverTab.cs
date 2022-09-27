using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// This packet draws an interface over the tab area.
    /// </summary>
    [PacketDef(PacketDefinition.InterfaceOverTab)]
    public sealed class SendInterfaceOverTab : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }


        /// <summary>
        /// Creates a new interface over tab packet payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        public SendInterfaceOverTab(int interfaceId)
        {
            InterfaceId = interfaceId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByteSubtrahend(InterfaceId);
        }

    }

}
