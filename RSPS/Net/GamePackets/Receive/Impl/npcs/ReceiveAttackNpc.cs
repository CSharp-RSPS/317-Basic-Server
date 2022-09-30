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

namespace RSPS.Net.GamePackets.Receive.Impl
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

            Npc? npc = WorldHandler.World.Npcs.ByIndex(npcIndex);

            if (npc == null)
            {
                return;
            }
            //TODO: Engage combat
        }

    }
}
