using RSPS.src.entity.Mobiles.Npcs;
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
    /// This packet is sent when a player attacks an NPC.
    /// </summary>
    [PacketInfo(72, 2)]
    public sealed class ReceiveAttackNpc : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            /*if (player.Disabled)
            {

            }*/
            int npcIndex = reader.ReadShortAdditional(false);

            World? world = WorldHandler.ResolveWorld(player);

            if (world == null)
            {
                return;
            }
            Npc? npc = world.Npcs.ByIndex(npcIndex);

            if (npc == null)
            {
                return;
            }
            //TODO: Engage combat
        }

    }
}
