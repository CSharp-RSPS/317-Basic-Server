using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Shows an interface in the chat box??????
    /// </summary>
    [PacketDef(SendPacketDefinition.SpawnAnimatedObject)]
    public sealed class SendSpawnAnimatedObject : IPacketPayloadBuilder
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
        /// The animation identifier
        /// </summary>
        public int AnimationId { get; private set; }


        /// <summary>
        /// Creates a new object spawn packet payload builder
        /// </summary>
        /// <param name="objectId">The object ID</param>
        /// <param name="objectType">The type of object</param>
        /// <param name="objectRotation">The rotation of the object</param>
        /// <param name="animationId">The animation identifier</param>
        public SendSpawnAnimatedObject(int objectId, int objectType, int objectRotation, int animationId)
        {
            ObjectId = objectId;
            ObjectType = objectType;
            ObjectRotation = objectRotation;
            AnimationId = animationId;
        }


        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByteSubtrahend(0); // ? postiion offset
            writer.WriteByteSubtrahend(ObjectType << 2 + ObjectRotation & 3);
            writer.WriteShortAdditional(AnimationId); // Not sure
        }

    }

}
