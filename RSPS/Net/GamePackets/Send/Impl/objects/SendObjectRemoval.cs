using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// This packet requests the client to remove an object.
    /// </summary>
    [PacketDef(PacketDefinition.ObjectRemoval)]
    public sealed class SendObjectRemoval : IPacketPayloadBuilder
    {

        /// <summary>
        /// The type of object
        /// </summary>
        public int ObjectType { get; private set; }

        /// <summary>
        /// The rotation of the object
        /// </summary>
        public int ObjectRotation { get; private set; }


        /// <summary>
        /// Creates a new send object removal packet payload builder
        /// </summary>
        /// <param name="objectType">The type of object</param>
        /// <param name="objectRotation">The rotation of the object</param>
        public SendObjectRemoval(int objectType, int objectRotation)
        {
            ObjectType = objectType;
            ObjectRotation = objectRotation;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(ObjectType << 2 + ObjectRotation & 3);
            writer.WriteByte(0); // not sure
        }

    }

}
