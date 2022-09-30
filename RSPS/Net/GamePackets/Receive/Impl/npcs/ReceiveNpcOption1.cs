using RSPS.Entities.Mobiles.Npcs;
using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using RSPS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive
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
