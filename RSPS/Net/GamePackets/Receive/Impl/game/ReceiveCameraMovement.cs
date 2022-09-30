using RSPS.Entities.Mobiles.Players;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
{
    /// <summary>
    /// This packet is sent when a player moves their game camera.
    /// </summary>
    [PacketInfo(86, 4)]
    public sealed class ReceiveCameraMovement : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int cameraY = reader.ReadShort();
            int cameraX = reader.ReadShortAdditional();
        }

    }
}
