using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Displays an interface in walkable mode.
    /// </summary>
    [PacketDef(PacketDefinition.WalkableInterface)]
    public sealed class SendWalkableInterface : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }


        /// <summary>
        /// Creates a new walkable interface packet payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        public SendWalkableInterface(int interfaceId)
        {
            InterfaceId = interfaceId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(InterfaceId, Packet.ByteOrder.LittleEndian);
        }

    }

}
