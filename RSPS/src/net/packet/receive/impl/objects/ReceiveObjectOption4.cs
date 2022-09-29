using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{

    /// <summary>
    /// Sent when a player uses the 4th option of an object.
    /// All object options likely support rotation, however only option 1 has it widespread available in clients
    /// Option 2 and 3 are sometimes to be found in clients but for option 4 I have not encounted it so far
    /// </summary>
    [PacketInfo(252, 6)]
    public sealed class ReceiveObjectOption4 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int objectId = reader.ReadShortAdditional();
            int objectY = reader.ReadShortAdditional();
            int objectX = reader.ReadShort();
        }

    }
}
