using RSPS.src.entity.npc;
using RSPS.src.entity.player;
using RSPS.src.net.packet.send.impl;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.receive.impl
{

    /// <summary>
    /// Sent when a player uses an item on an NPC.
    /// </summary>
    public sealed class ReceiveItemOnNpc : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int itemId = reader.ReadShort(Packet.ValueType.Additional);
            int npcIndex = reader.ReadShort(Packet.ValueType.Additional);
            int itemSlot = reader.ReadShort(Packet.ByteOrder.LittleEndian);
            int interfaceId = reader.ReadShort(Packet.ValueType.Additional);

            if (itemId < 0 || npcIndex < 0 || itemSlot < 0 || interfaceId < 0)
            {
                return;
            }
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
            //TODO walk to npc before executing action

            
            PacketHandler.SendPacket(player, new SendMessage("Nothing interesting happens."));
        }

    }
}
