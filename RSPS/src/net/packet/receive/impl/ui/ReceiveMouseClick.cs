using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.Connections;
using RSPS.src.Util.Annotations;

namespace RSPS.src.net.packet.receive.impl
{
    /// <summary>
    /// This packet is sent when a player clicks somewhere on the game screen.
    /// </summary>
    [PacketInfo(241, 3)] //TODO payload size not sure
    public class ReceiveMouseClick : IReceivePacket
    {

        public void ReceivePacket(Player player, PacketReader reader)
        {

        }

    }
}
