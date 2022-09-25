using RSPS.src.entity.npc;
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
    /// This packet is sent when a player clicks the second action of an NPC.
    /// </summary>
    [PacketInfo(17, 2)]
    public sealed class ReceiveNpcOption2 : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int npcIndex = reader.ReadShort(Packet.ValueType.Additional, Packet.ByteOrder.LittleEndian);

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
