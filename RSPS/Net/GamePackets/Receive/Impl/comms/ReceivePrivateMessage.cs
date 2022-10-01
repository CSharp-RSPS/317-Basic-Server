using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Comms.Messaging;
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
    /// Sent when a player sends a private message to another player.
    /// </summary>
    [PacketInfo(126, PacketHeaderType.VariableByte)]
    public sealed class ReceivePrivateMessage : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            long usernameAsLong = reader.ReadLong();
            int messageSize = reader.PayloadSize - 8;

            if (usernameAsLong <= 0 || messageSize <= 0)
            {
                return;
            }
            byte[] message = reader.ReadBytes(messageSize);
            PrivateMessageHandler.SendPrivateMessage(player, usernameAsLong, message, messageSize);
        }

    }
}
