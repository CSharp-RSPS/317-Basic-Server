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
    /// This packet is sent when a player requests to bank an X amount of items.
    /// </summary>
    [PacketInfo(135, 6)]
    public sealed class ReceiveBankXItemsPt1 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int slotId = reader.ReadShort(Packet.ByteOrder.LittleEndian);
            int interfaceId = reader.ReadShort(Packet.ValueType.Additional);
            int itemId = reader.ReadShort(Packet.ByteOrder.LittleEndian);

            switch (interfaceId)
            {
                //Add to bank
                case 5064:

                //Remove from bank
                case 5382:

                //Buy from shop
                case 3900:

                //Sell to shop
                case 3823:

                //Offer in trade
                case 3322:

                //Remove from trade
                case 3415:
                    PacketHandler.SendPacket(player, PacketDefinition.InputAmount);
                    break;
            }
        }

    }
}
