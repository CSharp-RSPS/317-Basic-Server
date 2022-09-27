using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Clears an interface's inventory.
    /// </summary>
    [PacketDef(PacketDefinition.ClearInventory)]
    public sealed class SendClearInventory : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }


        /// <summary>
        /// Creates a new clear inventory payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        public SendClearInventory(int interfaceId)
        {
            InterfaceId = interfaceId;
        }   

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortLittleEndian(InterfaceId);
        }

    }

}
