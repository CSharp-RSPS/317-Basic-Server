using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Clears an interface's inventory.
    /// </summary>
    [PacketDef(SendPacketDefinition.ClearInventory)]
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
