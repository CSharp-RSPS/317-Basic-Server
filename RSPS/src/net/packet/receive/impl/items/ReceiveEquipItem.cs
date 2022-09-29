﻿using RSPS.src.entity.Mobiles.Players;
using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{
    /// <summary>
    /// This is sent when a player equips an item in-game.
    /// </summary>
    [PacketInfo(41, 6)]
    public sealed class ReceiveEquipItem : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int itemId = reader.ReadShort(false);
            int slot = reader.ReadShortAdditional(false);
            int interfaceId = reader.ReadShortAdditional(false);

            if (itemId < 0 || slot < 0 || interfaceId < 0)
            {
                return;
            }
        }

    }
}
