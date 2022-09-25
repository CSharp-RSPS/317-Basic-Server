using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Causes a sidebar icon to start flashing.
    /// </summary>
    [PacketDef(PacketDefinition.FlashSidebar)]
    public sealed class SendFlashSidebar : IPacketPayloadBuilder
    {

        /// <summary>
        /// The sidebar ID
        /// </summary>
        public int SidebarId { get; private set; }


        /// <summary>
        /// Creates a new flash sidebar packet payload builder
        /// </summary>
        /// <param name="sidebarId">The sidebar ID</param>
        public SendFlashSidebar(int sidebarId)
        {
            SidebarId = sidebarId;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(SidebarId, Packet.ValueType.Subtrahend);
        }

        /*
         * Sidebar ID	Icon
0	Attack type
-1	Stats
-2	Quests
-3	Inventory
-4	Wearing
-5	Prayer
-6	Magic
-7	EMPTY
-8	Friends list
-9	Ignore list
-10	Log out
-11	Settings
-12	Emotes
-13	Music
         * */

    }

}
