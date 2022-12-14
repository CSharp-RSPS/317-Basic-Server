using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Displays a hint icon.
    /// </summary>
    [PacketDef(SendPacketDefinition.DisplayHintIcon)]
    public sealed class SendDisplayHintIcon : IPacketVariablePayloadBuilder
    {

        /// <summary>
        /// The icon type
        /// </summary>
        public int IconType { get; private set; }


        /// <summary>
        /// Creates a new display hint icon payload builder
        /// </summary>
        /// <param name="iconType">The icon type</param>
        public SendDisplayHintIcon(int iconType)
        {
            IconType = iconType;
        }

        public int GetPayloadSize()
        {
            return 1;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(IconType); // Values supported = 1, 2, 3, 4, 5, 6

            if (IconType == 1)
            {
                writer.WriteShort(IconType); // Not sure
            }
        }

    }

}
