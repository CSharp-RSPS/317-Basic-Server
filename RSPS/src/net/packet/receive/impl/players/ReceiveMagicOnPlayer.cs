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
    /// This packet is sent when the player attempts to cast magic onto another.
    /// </summary>
    [PacketInfo(249, 4)]
    public sealed class ReceiveMagicOnPlayer : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            int playerIndex = reader.ReadShort(Packet.ValueType.Additional);
            int spellId = reader.ReadShort(Packet.ByteOrder.LittleEndian);

            World? world = WorldHandler.ResolveWorld(player);

            if (world == null)
            {
                return;
            }
            Player? other = world.Players.ByWorldIndex(playerIndex);

            if (other == null)
            {
                return;
            }
        }

    }
}
