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
    /// This packet is sent when a player clicks the follow option on another player.
    /// </summary>
    [PacketInfo(39, 2)]
    public sealed class ReceiveFollow : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int playerIndex = reader.ReadShort(false, Packet.ByteOrder.LittleEndian);

            if (playerIndex < 0)
            {
                return;
            }
            World? world = WorldHandler.ResolveWorld(player);

            if (world == null)
            {
                return;
            }
            Player? leader = world.Players.ByWorldIndex(playerIndex);

            if (leader == null || !leader.Position.isViewableFrom(player.Position))
            {
                return;
            }
            //TODO follow
        }

    }
}
