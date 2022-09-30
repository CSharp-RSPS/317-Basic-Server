using RSPS.Entities.Mobiles.Npcs;
using RSPS.Entities.movement.Locations;
using RSPS.Entities.Mobiles.Players;
using RSPS.Util.Attributes;
using RSPS.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
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
        /// Creates a new npc updating payload writer
        /// </summary>
        /// <param name="player">The player we're updating npc's for</param>
        /// <param name="npcs">The npc's in the world at the time of update</param>
        public SendNpcUpdating(Player player)
        {
            Player = player;
        }

        public int GetPayloadSize()
        {
            return 2048; // 1024
        }

        public void WritePayload(PacketWriter writer)
        {
            PacketWriter stateBlock = new(1024); // 726

            writer.SetAccessType(Packet.AccessType.BitAccess);
            writer.WriteBits(8, Player.LocalNpcs.Count);

            foreach (Npc npc in Player.LocalNpcs.ToArray())
            {
                if (npc.Position.IsWithinDistance(Player.Position) 
                    && !npc.Movement.Teleported
                    && npc.NpcSpawn.Spawned)
                {
                    npc.Movement.Update(npc, writer);

                    if (npc.UpdateRequired)
                    {
                        updateState(stateBlock, npc);
                    }
                }
                else
                {
                    // Remove the NPC from the local list.
                    //writer.WriteBit(true);
                    writer.WriteBits(1, 1);
                    writer.WriteBits(2, 3);

                    Player.LocalNpcs.Remove(npc);
                    npc.LocalPlayers.Remove(Player);
                }
            }
            foreach (Npc npc in WorldHandler.World.Npcs.Entities)
            {
                if (npc == null || !npc.NpcSpawn.Spawned 
                    || Player.LocalNpcs.Contains(npc)
                    || !npc.Position.IsWithinDistance(Player.Position))
                {
                    continue;
                }
                Player.LocalNpcs.Add(npc);
                npc.LocalPlayers.Add(Player);

                // Add the NPC for the player
                writer.WriteBits(14, npc.WorldIndex);
                Position delta = Position.Delta(Player.Position, npc.Position);
                writer.WriteBits(5, delta.Y);
                writer.WriteBits(5, delta.X);
                writer.WriteBits(1, 1); // writer.WriteBit(npc.HasUpdates);
                writer.WriteBits(12, npc.Id); // 14
                writer.WriteBits(1, npc.UpdateRequired ? 1 : 0); //TODO change to new updating

                if (npc.UpdateRequired)
                {
                    updateState(stateBlock, npc);
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
