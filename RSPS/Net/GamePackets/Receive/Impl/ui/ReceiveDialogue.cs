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
    /// Received by the server when a button is pressed in a Chat interface.
    /// </summary>
    [PacketInfo(40, 2)]
    public sealed class ReceiveDialogue : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int frameId = reader.ReadShort();

            if (frameId < 0)
            {
                return;
            }
            PacketHandler.SendPacket(player, PacketDefinition.ClearScreen);

            // TODO
        }

    }
}
