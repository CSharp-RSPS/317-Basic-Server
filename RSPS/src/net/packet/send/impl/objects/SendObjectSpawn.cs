using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// This packet requests the client to spawn an object.
    /// </summary>
    [PacketDef(PacketDefinition.ObjectSpawn)]
    public sealed class SendObjectSpawn : IPacketPayloadBuilder
    {

        /// <summary>
        /// The object ID
        /// </summary>
        public int ObjectId { get; private set; }

        /// <summary>
        /// The type of object
        /// </summary>
        public int ObjectType { get; private set; }

        /// <summary>
        /// The rotation of the object
        /// </summary>
        public int ObjectRotation { get; private set; }


        /// <summary>
        /// Creates a new object spawn packet payload builder
        /// </summary>
        /// <param name="objectId">The object ID</param>
        /// <param name="objectType">The type of object</param>
        /// <param name="objectRotation">The rotation of the object</param>
        public SendObjectSpawn(int objectId, int objectType, int objectRotation)
        {
            ObjectId = objectId;
            ObjectType = objectType;
            ObjectRotation = objectRotation;
        }

        public void WritePayload(PacketWriter writer)
        { //TODO: Not correct yet
            writer.WriteByteSubtrahend(0);
            //writer.WriteByte(ObjectId, Packet.ByteOrder.LittleEndian);
            writer.WriteByteSubtrahend(ObjectType << 2 + ObjectRotation & 3);
        }

    }

}
