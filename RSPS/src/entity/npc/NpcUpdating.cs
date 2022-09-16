using RSPS.src.entity.player;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.npc
{
    public static class NpcUpdating
    {

        public static void Update(Player player)
        {
            PacketWriter outPacket = Packet.CreatePacketWriter(5000);//gets 32 players
            PacketWriter stateBlock = Packet.CreatePacketWriter(2048);

            outPacket.WriteVariableShortHeader(player.PlayerConnection.NetworkEncryptor, 65);
            outPacket.SetAccessType(Packet.AccessType.BIT_ACCESS);

            // Update the NPCs in the local list.
			outPacket.WriteBits(8, player.LocalNpcs.Count);
			foreach (Npc npc in player.LocalNpcs)
			{
				if (npc.Position.isViewableFrom(player.Position) && npc.Visible)
				{
					NpcUpdating.UpdateNpcMovement(outPacket, npc);
					if (npc.UpdateRequired)
					{
						NpcUpdating.updateState(stateBlock, npc);
					}
				}
				else
				{
					// Remove the NPC from the local list.
					outPacket.WriteBit(true);
					outPacket.WriteBits(2, 3);
					player.LocalNpcs.Remove(npc);
				}
			}

			// Update the local NPC list itself.
			for (int i = 0; i < World.npcs.Count; i++)
			{
				Npc npc = (Npc)World.npcs[i];
				if (npc == null || player.LocalNpcs.Contains(npc) || !npc.Visible)
				{
					continue;
				}
				if (npc.Position.isViewableFrom(player.Position))
				{
					player.LocalNpcs.Add(npc);
					addNpc(outPacket, player, npc);
					if (npc.UpdateRequired)
					{
						NpcUpdating.updateState(stateBlock, npc);
					}
				}
			}

			// Append the update block to the packet if need be.
			if (stateBlock.PayloadPosition > 0)
			{
				outPacket.WriteBits(14, 16383);
				outPacket.SetAccessType(Packet.AccessType.BYTE_ACCESS);
				outPacket.WriteBytes(stateBlock.Payload, stateBlock.PayloadPosition);
			}
			else
			{
				outPacket.SetAccessType(Packet.AccessType.BYTE_ACCESS);
			}

			outPacket.FinishVariableShortHeader();
			Program.SendGlobalByes(player.PlayerConnection, outPacket.Payload, outPacket.PayloadPosition);

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
			outPacket.WriteBits(14, npc.Index);
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
				block.writeShort(mask, Packet.ByteOrder.LITTLE);
			}
			else
			{
				block.WriteByte(mask);
			}

			// TODO: Append the NPC update blocks.
		}

	}
}
