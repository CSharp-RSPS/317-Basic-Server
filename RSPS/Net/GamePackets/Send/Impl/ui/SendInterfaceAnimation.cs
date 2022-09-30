using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Sets an interface's model animation.
    /// </summary>
    [PacketDef(PacketDefinition.InterfaceAnimation)]
    public sealed class SendInterfaceAnimation : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// The animation ID
        /// </summary>
        public int AnimationId { get; private set; }


        /// <summary>
        /// Creates a new interface animation packet payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        /// <param name="animationId">The animation ID</param>
        public SendInterfaceAnimation(int interfaceId, int animationId)
        {
            InterfaceId = interfaceId;
            AnimationId = animationId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(InterfaceId);
            writer.WriteShort(AnimationId);
        }

    }

}
