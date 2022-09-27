using RSPS.src.entity.player;
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

            World? world = WorldHandler.ResolveWorld(player);

            if (world == null)
            {
                return;
            }
            Player? other = world.Players.ByWorldIndex(playerIndex);

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
