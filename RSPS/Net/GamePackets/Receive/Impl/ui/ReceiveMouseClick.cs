using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.Entities.Mobiles.Players;
using RSPS.Net.Connections;
using RSPS.Util.Attributes;

namespace RSPS.Net.GamePackets.Receive.Impl
{
    /// <summary>
    /// This packet is sent when a player clicks somewhere on the game screen.
    /// </summary>
    [PacketInfo(241, 3)] //TODO payload size not sure
    public class ReceiveMouseClick : IReceivePacket
    {

        public void ReceivePacket(Player player, PacketReader reader)
        {
            int value = reader.ReadInt();
        }

    }
}
