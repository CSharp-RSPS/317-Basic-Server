using RSPS.src.entity.player;
using RSPS.src.net.packet.send;
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
    /// This packet is sent when a player presses the close, exit or cancel button on an interface.
    /// </summary>
    [PacketInfo(130, 0)]
    public sealed class ReceiveCloseWindow : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            PacketHandler.SendPacket(player, PacketDefinition.ClearScreen);
        }

    }
}
