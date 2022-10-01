using RSPS.Entities.Mobiles.Players;
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
    /// This packet is sent when a player changes their privacy options (i.e. public chat).
    /// </summary>
    [PacketInfo(95, 3)]
    public sealed class ReceivePrivacyOptions : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int publicChatOptions = reader.ReadByte(false);
            int privateChatOptions = reader.ReadByte(false);
            int tradeOptions = reader.ReadByte(false);

            Debug.WriteLine("public: " + publicChatOptions + ", private: " + privateChatOptions + ", trade: " + tradeOptions);
            Debug.WriteLine("");
        }

    }
}
