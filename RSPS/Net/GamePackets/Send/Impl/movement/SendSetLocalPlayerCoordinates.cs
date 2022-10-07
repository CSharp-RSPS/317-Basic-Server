using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Set local player coordinates
    /// </summary>
    [PacketDef(SendPacketDefinition.SetLocalPlayerCoordinates)]
    public sealed class SendSetLocalPlayerCoordinates : IPacketPayloadBuilder
    {

        /// <summary>
        /// The local X coordinate
        /// </summary>
        public int LocalX { get; private set; }

        /// <summary>
        /// The local Y coordinate
        /// </summary>
        public int LocalY { get; private set; }


        /// <summary>
        /// Creates a new set local player coordinates
        /// </summary>
        /// <param name="localX">The local X coordinate</param>
        /// <param name="localY">The local Y coordinate</param>
        public SendSetLocalPlayerCoordinates(int localX, int localY)
        {
            LocalX = localX;
            LocalY = localY;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByteNegated(LocalY);
            writer.WriteByteNegated(LocalX);
        }

    }

}
