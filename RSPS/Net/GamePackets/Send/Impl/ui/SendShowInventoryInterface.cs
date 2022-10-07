using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Show inventory interface
    /// </summary>
    [PacketDef(SendPacketDefinition.ShowInventoryInterface)]
    public sealed class SendShowInventoryInterface : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface identifier
        /// </summary>
        public int InterfaceId { get; private set; }


        /// <summary>
        /// Shows an inventory interface
        /// </summary>
        /// <param name="interfaceId">The interface identifier</param>
        public SendShowInventoryInterface(int interfaceId)
        {
            InterfaceId = interfaceId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortLittleEndian(InterfaceId);
        }

    }

}
