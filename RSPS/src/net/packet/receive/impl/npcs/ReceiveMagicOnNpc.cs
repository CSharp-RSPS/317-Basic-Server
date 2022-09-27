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
    /// Sent when a player attempts to use a magic attack on an NPC.
    /// </summary>
    [PacketInfo(131, 4)]
    public sealed class ReceiveMagicOnNpc : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int npcIndex = reader.ReadShortAdditionalLittleEndian();
            int spellId = reader.ReadShortAdditional();

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
        }

    }
}
