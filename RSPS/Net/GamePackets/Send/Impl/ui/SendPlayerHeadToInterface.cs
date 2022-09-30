using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// This packet sends a players head to an interface
    /// </summary>
    [PacketDef(PacketDefinition.PlayerHeadToInterface)]
    public sealed class SendPlayerHeadToInterface : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }


        /// <summary>
        /// Creates a new player head to interface packet payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        public SendPlayerHeadToInterface(int interfaceId)
        {
            InterfaceId = interfaceId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortAdditionalLittleEndian(InterfaceId);
        }

    }

}
