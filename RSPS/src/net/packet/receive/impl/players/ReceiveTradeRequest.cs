﻿using RSPS.src.entity.npc;
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
    /// This packet is sent when a player answers a trade request from another player.
    /// </summary>
    [PacketInfo(139, 2)]
    public sealed class ReceiveTradeRequest : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int playerIndex = reader.ReadShort(Packet.ByteOrder.LittleEndian);

            World? world = WorldHandler.ResolveWorld(player);

            if (world == null)
            {
                return;
            }
            Player? other = world.Players.ByWorldIndex(playerIndex);

            if (other == null || !other.Position.isViewableFrom(player.Position))
            {
                return;
            }
        }

    }
}
