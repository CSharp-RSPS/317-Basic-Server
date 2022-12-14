using RSPS.Entities.Mobiles.Npcs;
using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Receive.Impl
{

    /// <summary>
    /// Sent when a player uses an item on an NPC.
    /// </summary>
    public sealed class ReceiveItemOnNpc : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int itemId = reader.ReadShortAdditional();
            int npcIndex = reader.ReadShortAdditional();
            int itemSlot = reader.ReadShortLittleEndian();
            int interfaceId = reader.ReadShortAdditional();

            if (itemId < 0 || npcIndex < 0 || itemSlot < 0 || interfaceId < 0)
            {
                return;
            }
            Npc? npc = WorldHandler.World.Npcs.ByIndex(npcIndex);

            if (npc == null)
            {
                return;
            }
            //TODO walk to npc before executing action

            
            PacketHandler.SendPacket(player, new SendMessage("Nothing interesting happens."));
        }

    }
}
