using RSPS.Entities.Mobiles.Players;
using RSPS.Game.UI.Buttons;
using RSPS.Net.Connections;
using RSPS.Net.GamePackets.Send;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
{
    /// <summary>
    /// This is sent when a player clicks a button in-game, with the id of the button being clicked.
    /// </summary>
    [PacketInfo(185, 2)]
    public sealed class ReceiveButtonClick : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int buttonId = reader.ReadShort();

            if (buttonId < 0)
            {
                return;
            }
            ButtonHandler.HandleButtonClick(player, buttonId);
        }
    }
}
