using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Changes the zoom and rotation of the interface id's media given.
    /// </summary>
    [PacketDef(SendPacketDefinition.InterfaceModelRotation)]
    public sealed class SendInterfaceModelRotation : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// The zoom
        /// </summary>
        public int Zoom { get; private set; }

        /// <summary>
        /// The rotation 1
        /// </summary>
        public int Rotation1 { get; private set; }

        /// <summary>
        /// The rotaiton 2
        /// </summary>
        public int Rotation2 { get; private set; }


        /// <summary>
        /// Creates a new interface model rotation packet payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        /// <param name="zoom">The zoom</param>
        /// <param name="rotation1">The rotation 1</param>
        /// <param name="rotation2">The rotation 2</param>
        public SendInterfaceModelRotation(int interfaceId, int zoom, int rotation1, int rotation2)
        {
            InterfaceId = interfaceId;
            Zoom = zoom;
            Rotation1 = rotation1;
            Rotation2 = rotation2;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortAdditional(Zoom);
            writer.WriteShort(InterfaceId);
            writer.WriteShort(Rotation1);
            writer.WriteShortAdditionalLittleEndian(Rotation2);
        }

    }

}
