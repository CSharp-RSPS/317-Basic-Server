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
    /// Sent when a player attempts to use a magic attack on an NPC.
    /// </summary>
    [PacketInfo(131, 4)]
    public sealed class ReceiveMagicOnNpc : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int npcIndex = reader.ReadShortAdditionalLittleEndian();
            int spellId = reader.ReadShortAdditional();

            Npc? npc = WorldHandler.World.Npcs.ByIndex(npcIndex);

            if (npc == null)
            {
                return;
            }
        }

    }
}
