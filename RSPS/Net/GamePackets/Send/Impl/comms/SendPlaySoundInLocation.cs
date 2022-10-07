using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Play sound in location.
    /// </summary>
    [PacketDef(SendPacketDefinition.PlaySoundInLocation)]
    public sealed class SendPlaySoundInLocation : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(0); // ?
            writer.WriteShort(0); // ?
            writer.WriteByte(0); // ?
        }

    }

}
