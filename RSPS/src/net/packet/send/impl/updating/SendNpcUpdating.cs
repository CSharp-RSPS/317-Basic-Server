using RSPS.src.entity.Mobiles.Npcs;
using RSPS.src.entity.movement.Locations;
using RSPS.src.entity.player;
using RSPS.src.Util.Annotations;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// NPC updating
    /// </summary>
    [PacketDef(PacketDefinition.NPCUpdating)]
    public sealed class SendNpcUpdating : IPacketVariablePayloadBuilder
    {

        private static readonly int REGION_NPCS_LIMIT = 255;

        private static readonly int NEW_NPCS_PER_CYCLE = 45;

        /// <summary>
        /// The player we're updating npc's for
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// The npc's in the world at the time of update
        /// </summary>
        public List<Npc> Npcs { get; private set; }


        /// <summary>
        /// Creates a new npc updating payload writer
        /// </summary>
        /// <param name="player">The player we're updating npc's for</param>
        /// <param name="npcs">The npc's in the world at the time of update</param>
        public SendNpcUpdating(Player player, List<Npc> npcs)
        {
            Player = player;
            Npcs = npcs;
        }

        public int GetPayloadSize()
        {
            return 2048;
        }

        public void WritePayload(PacketWriter writer)
        {
            PacketWriter stateBlock = new(1024);

            writer.SetAccessType(Packet.AccessType.BitAccess);
            writer.WriteBits(8, Player.LocalNpcs.Count);

            foreach (Npc npc in Player.LocalNpcs)
            {
                if (npc.Position.IsWithinDistance(Player.Position) && npc.NpcSpawn.Spawned)
                {
                    UpdateNpcMovement(writer, npc);

                    if (npc.UpdateRequired)
                    {
                        updateState(stateBlock, npc);
                    }
                }
                else
                {
                    // Remove the NPC from the local list.
                    writer.WriteBit(true);
                    writer.WriteBits(2, 3);
                    Player.LocalNpcs.Remove(npc);
                }
            }
            // Update the local NPC list itself.
            for (int i = 0; i < Npcs.Count; i++)
            {
                Npc npc = (Npc)Npcs[i];
                if (npc == null || Player.LocalNpcs.Contains(npc) || !npc.NpcSpawn.Spawned)
                {
                    continue;
                }
                if (npc.Position.IsWithinDistance(Player.Position))
                {
                    Player.LocalNpcs.Add(npc);
                    addNpc(writer, Player, npc);
                    if (npc.UpdateRequired)
                    {
                        updateState(stateBlock, npc);
                    }
                }
            }
            // Append the update block to the packet if need be.
            if (stateBlock.Pointer > 0)
            {
                writer.WriteBits(14, 16383);
                writer.SetAccessType(Packet.AccessType.ByteAccess);
                writer.WriteBytes(stateBlock.Buffer, stateBlock.Pointer);
            }
            else
            {
                writer.SetAccessType(Packet.AccessType.ByteAccess);
            }
        }
        /**
		 * Adds the NPC to the clientside local list.
		 * 
		 * @param out
		 *            The buffer to write to
		 * @param player
		 *            The player
		 * @param npc
		 *            The NPC being added
		 */
        private static void addNpc(PacketWriter outPacket, Player player, Npc npc)
        {
            outPacket.WriteBits(14, npc.WorldIndex);
            Position delta = Position.Delta(player.Position, npc.Position);
            outPacket.WriteBits(5, delta.Y);
            outPacket.WriteBits(5, delta.X);
            outPacket.WriteBit(npc.UpdateRequired);
            outPacket.WriteBits(12, npc.Id);
            outPacket.WriteBit(true);
        }

        /**
		 * Updates the movement of a NPC for this cycle.
		 * 
		 * @param out
		 *            The buffer to write to
		 * @param npc
		 *            The NPC to update
		 */
        private static void UpdateNpcMovement(PacketWriter outPacket, Npc npc)
        {
            if (npc.PrimaryDirection == -1)
            {
                if (npc.UpdateRequired)
                {
                    outPacket.WriteBit(true);
                    outPacket.WriteBits(2, 0);
                }
                else
                {
                    outPacket.WriteBit(false);
                }
            }
            else
            {
                outPacket.WriteBit(true);
                outPacket.WriteBits(2, 1);
                outPacket.WriteBits(3, npc.PrimaryDirection);
                outPacket.WriteBit(true);
            }
        }

        /**
		 * Updates the state of the NPC to the given update block.
		 * 
		 * @param block
		 *            The update block to append to
		 * @param npc
		 *            The NPC to update
		 */
        private static void updateState(PacketWriter block, Npc npc)
        {
            int mask = 0x0;

            // TODO: NPC update masks.

            if (mask >= 0x100)
            {
                mask |= 0x40;
                block.WriteShortLittleEndian(mask);
            }
            else
            {
                block.WriteByte(mask);
            }

            // TODO: Append the NPC update blocks.
        }

    }

}
