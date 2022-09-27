using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// This packet changes the color of an interface that is text.
    /// </summary>
    [PacketDef(PacketDefinition.InterfaceColor)]
    public sealed class SendInterfaceColor : IPacketPayloadBuilder
    {

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }

        /// <summary>
        /// The color
        /// </summary>
        public int Color { get; private set; }


        /// <summary>
        /// Creates a new interface color packet payload builder
        /// </summary>
        /// <param name="interfaceId">The interface ID</param>
        /// <param name="color">The color</param>
        public SendInterfaceColor(int interfaceId, int color)
        {
            InterfaceId = interfaceId;
            Color = color;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortAdditionalLittleEndian(InterfaceId);
            writer.WriteShortAdditionalLittleEndian(Color);
        }

        /*
         * Color	Code
Green	0x3366
Yellow	0x33FF66
Red	0x6000
         * */

    }

}
