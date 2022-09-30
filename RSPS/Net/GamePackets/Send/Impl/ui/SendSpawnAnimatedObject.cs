using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Shows an interface in the chat box??????
    /// </summary>
    [PacketDef(PacketDefinition.SpawnAnimatedObject)]
    public sealed class SendSpawnAnimatedObject : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            throw new NotImplementedException();
        }

    }

}
