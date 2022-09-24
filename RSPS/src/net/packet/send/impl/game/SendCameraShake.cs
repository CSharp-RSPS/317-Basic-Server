using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Causes the camera to shake.
    /// </summary>
    [PacketDef(PacketDefinition.CameraShake)]
    public sealed class SendCameraShake : IPacketPayloadBuilder
    {


        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(1);//max of 4 Array Index
            writer.WriteByte(0);//Affects All values
            writer.WriteByte(1);//Y Move Camera
            writer.WriteByte(0);
        }

    }
}
