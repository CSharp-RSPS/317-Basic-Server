using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// This packet overlays an interface in the inventory area. This is used in trading and staking.
    /// </summary>
    [PacketDef(PacketDefinition.InventoryOverlay)]
    public sealed class SendInventoryOverlay : IPacketPayloadBuilder
    {

        /// <summary>
        /// The ID of the interface to open
        /// </summary>
        public int InterfaceToOpenId { get; private set; }

        /// <summary>
        /// The ID of the interface to overlay the inventory area with
        /// </summary>
        public int InterfaceToOverlayId { get; private set; }


        /// <summary>
        /// Creates a new inventory overlay packet payload builder
        /// </summary>
        /// <param name="interfaceToOpenId">The ID of the interface to open</param>
        /// <param name="interfaceToOverlayId">The ID of the interface to overlay the inventory area with</param>
        public SendInventoryOverlay(int interfaceToOpenId, int interfaceToOverlayId)
        {
            InterfaceToOpenId = interfaceToOpenId;
            InterfaceToOverlayId = interfaceToOverlayId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortAdditional(InterfaceToOpenId);
            writer.WriteShort(InterfaceToOverlayId);
        }

    }

}
