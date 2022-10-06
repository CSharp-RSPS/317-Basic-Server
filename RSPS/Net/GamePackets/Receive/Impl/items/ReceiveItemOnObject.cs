using RSPS.Entities.Mobiles.Players;
using RSPS.Game.UI;
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
    /// This packet is sent when a player uses an item on object.
    /// Contrary to what most servers have, the packet is 14 and not 12 in size because the client also sends an object rotation.
    /// </summary>
    [PacketInfo(192, 14)]
    public sealed class ReceiveItemOnObject : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int interfaceId = reader.ReadShortAdditional();
            int objectId = reader.ReadShortLittleEndian();
            int objectY = reader.ReadShortAdditionalLittleEndian();
            int itemSlotId = reader.ReadShortLittleEndian();
            int objectX = reader.ReadShortAdditionalLittleEndian();
            int itemId = reader.ReadShort();
            int rotation = reader.ReadShortAdditionalLittleEndian();

            switch (interfaceId)
            {
                case Interfaces.Inventory: // Inventory
                    break;
            }
            PacketHandler.SendPacket(player, new SendMessage("Nothing interesting happens."));
        }

    }
}
