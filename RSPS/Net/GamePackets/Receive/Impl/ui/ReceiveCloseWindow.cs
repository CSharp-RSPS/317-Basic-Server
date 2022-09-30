using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets.Send;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
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
