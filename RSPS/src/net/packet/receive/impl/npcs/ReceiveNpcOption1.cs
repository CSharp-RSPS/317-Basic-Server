using RSPS.src.entity.Mobiles.Npcs;
using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Util.Annotations;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive
{

    /// <summary>
    /// This packet is sent when a player clicks the first option of an NPC.
    /// </summary>
    [PacketInfo(155, 2)]
    public sealed class ReceiveNpcOption1 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int npcIndex = reader.ReadShortLittleEndian();

            Npc? npc = WorldHandler.World.Npcs.ByIndex(npcIndex);

            if (npc == null)
            {
                return;
            }
        }

    }
}
