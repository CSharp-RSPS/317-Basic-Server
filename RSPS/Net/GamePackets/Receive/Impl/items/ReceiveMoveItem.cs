using RSPS.Entities.Mobiles.Players;
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
    /// This packet is sent when a player moves an item from one slot to another.
    /// </summary>
    [PacketInfo(214, 7)]
    public sealed class ReceiveMoveItem : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int interfaceId = reader.ReadShortAdditionalLittleEndian();
            int insertMode = reader.ReadByte();
            int startingSlot = reader.ReadShortAdditionalLittleEndian();
            int newSlot = reader.ReadShortLittleEndian();

            switch (interfaceId)
            {
                case 3214: //Inventory
                case 5064: //Inventory
                    break;

                case 5382: // Bank
                    break;
            }
        }

    }
}
