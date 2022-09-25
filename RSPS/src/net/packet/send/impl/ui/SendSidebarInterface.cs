using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// This packet assigns an interface to one of the tabs in the game sidebar.
    /// </summary>
    [PacketDef(PacketDefinition.SendSidebarInterface)]
    public sealed class SendSidebarInterface : IPacketPayloadBuilder
    {

        /// <summary>
        /// The sidebar ID
        /// </summary>
        public int SidebarId { get; private set; }

        /// <summary>
        /// The interface ID
        /// </summary>
        public int InterfaceId { get; private set; }


        /// <summary>
        /// Creates a new sidebar interface packet payload builder
        /// </summary>
        /// <param name="sidebarId">The sidebar ID</param>
        /// <param name="interfaceID">The interface ID</param>
        public SendSidebarInterface(int sidebarId, int interfaceID)
        {
            SidebarId = sidebarId;
            InterfaceId = interfaceID;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(InterfaceId);
            writer.WriteByte(SidebarId, Packet.ValueType.Additional);
        }

        /*
         * Value	Icon	Norm. ID
0	Attack type	2433
1	Stats	3917
2	Quests	638
3	Inventory	3213
4	Wearing	1644
5	Prayer	5608
6	Magic	1151
7	EMPTY	N/A
8	Friends list	5065
9	Ignore list	5715
10	Log out	2449
11	Settings	4445
12	Emotes	147
13	Music	6299

         * */
    }
}
