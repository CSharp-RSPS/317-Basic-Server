using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Place the head of an NPC on an interface
    /// </summary>
    [PacketDef(PacketDefinition.NPCHeadOnInterface)]
    public sealed class SendNpcHeadOnInterface : IPacketPayloadBuilder
    {

        /// <summary>
        /// The NPC ID
        /// </summary>
        public int NpcId { get; private set; }

        /// <summary>
        /// The 'widget' ID for where you wish to place the head on the interface
        /// </summary>
        public int WidgetId { get; private set; }


        /// <summary>
        /// Creates a new NPC head on interface packet payload builder
        /// </summary>
        /// <param name="npcId">The NPC ID</param>
        /// <param name="widgetId">The 'widget' ID for where you wish to place the head on the interface</param>
        public SendNpcHeadOnInterface(int npcId, int widgetId)
        {
            NpcId = npcId;
            WidgetId = widgetId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortAdditionalLittleEndian(NpcId);
            writer.WriteShortAdditionalLittleEndian(WidgetId);
        }

    }

}
