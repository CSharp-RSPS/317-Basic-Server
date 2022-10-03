using RSPS.Entities.Mobiles.Players;
using RSPS.Game.UI;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using RSPS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
{

    /// <summary>
    /// This packet is sent when a player uses an item on another player.
    /// </summary>
    [PacketInfo(14, 8)]
    public sealed class ReceiveItemOnPlayer : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int interfaceId = reader.ReadShortAdditional();
            int playerIndex = reader.ReadShort();
            int itemId = reader.ReadShort();
            int itemSlot = reader.ReadShortLittleEndian();

            Player? other = WorldHandler.World.Players.ByWorldIndex(playerIndex);

            if (other == null)
            {
                return;
            }
            switch (interfaceId)
            {
                case Interfaces.Inventory: // Inventory
                    break;
            }
            PacketHandler.SendPacket(player, new SendMessage("Nothing interesting happens."));
        }

    }
}
