using RSPS.src.entity.Mobiles.Players;
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
    /// This packet is sent when a player enters an X amount of items they want to bank.
    /// </summary>
    [PacketInfo(208, 4)]
    public sealed class ReceiveBankXItemsPt2 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int amount = reader.ReadInt();
            int interfaceId = 0; //TODO

            switch (interfaceId)
            {
                case 5064: // Add to bank
                    break;

                case 5382: // Remove from bank
                    break;

                case 3900: // Buy from shop
                    break;

                case 3823: // Sell to shop
                    break;

                case 3322: // Offer in trade
                    break;

                case 3415: // Remove from trade
                    break;
            }
        }

    }
}
