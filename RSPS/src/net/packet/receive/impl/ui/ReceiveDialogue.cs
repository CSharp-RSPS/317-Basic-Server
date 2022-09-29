using RSPS.src.entity.Mobiles.Players;
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
