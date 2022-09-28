using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
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
        /// The 'slot' ID for where you wish to place the head
        /// </summary>
        public int Slot { get; private set; }


        /// <summary>
        /// Creates a new NPC head on interface packet payload builder
        /// </summary>
        /// <param name="npcId">The NPC ID</param>
        /// <param name="slot">The 'slot' ID for where you wish to place the head</param>
        public SendNpcHeadOnInterface(int npcId, int slot)
        {
            NpcId = npcId;
            Slot = slot;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortAdditionalLittleEndian(NpcId);
            writer.WriteShortAdditionalLittleEndian(Slot);
        }

    }

}
