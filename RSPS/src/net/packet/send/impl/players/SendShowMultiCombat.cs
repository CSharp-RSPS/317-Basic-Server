using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Shows the player that they are in a multi-combat zone.
    /// </summary>
    [PacketDef(PacketDefinition.ShowMultiCombat)]
    public sealed class SendShowMultiCombat : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            throw new NotImplementedException();
        }

    }

}
