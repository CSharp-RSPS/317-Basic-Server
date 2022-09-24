using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Adds a player option to the right click menu of player(s).
    /// </summary>
    [PacketDef(PacketDefinition.PlayerOption)]
    public sealed class SendPlayerOption : IPacketVariablePayloadBuilder
    {


        public int GetPayloadSize()
        {
            throw new NotImplementedException();
        }

        public void WritePayload(PacketWriter writer)
        {
            throw new NotImplementedException();
        }

    }

}
