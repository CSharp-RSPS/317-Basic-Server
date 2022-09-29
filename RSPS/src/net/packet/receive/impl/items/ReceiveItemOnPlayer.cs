using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util.Annotations;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
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

            Player? other = WorldHandler.World.Players.ByPlayerIndex(playerIndex);

            if (other == null)
            {
                return;
            }
            switch (interfaceId)
            {
                case 3214: // Inventory
                    break;
            }
            PacketHandler.SendPacket(player, new SendMessage("Nothing interesting happens."));
        }

    }
}
