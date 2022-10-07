using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Draw a graphic at a given x/y position after a delay.
    /// </summary>
    [PacketDef(SendPacketDefinition.DrawGraphicAtPosition)]
    public sealed class SendDrawGraphic : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(0); // ?
            writer.WriteShort(0); // ?
            writer.WriteByte(0); // ?
            writer.WriteShort(0); // ?
        }

    }

}
