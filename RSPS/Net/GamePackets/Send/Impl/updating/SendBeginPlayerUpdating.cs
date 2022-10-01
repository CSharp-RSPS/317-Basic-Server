using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.Mobiles.Players.Skills;
using RSPS.Entities.movement.Locations;
using RSPS.Net.Connections;
using RSPS.Util.Attributes;
using RSPS.Worlds;
using System.Diagnostics;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Begins the player update procedure
    /// </summary>
    [PacketDef(PacketDefinition.BeginPlayerUpdating)]
    public sealed class SendBeginPlayerUpdating : IPacketVariablePayloadBuilder
    {

        private static readonly int MAX_APPEARANCE_BUFFER_SIZE = 58;//full size of buffer per player

        private static readonly int RegionPlayersLimit = 255;

        private static readonly int NewPlayersPerCycle = 45;

        /// <summary>
        /// The player we're updating for
        /// </summary>
        public Player Player { get; private set; }


        /// <summary>
        /// Creates a new player updating payload builder
        /// </summary>
        /// <param name="player">The player we're updating for</param>
        /// <param name="players">The player in the world at the time of update</param>
        public SendBeginPlayerUpdating(Player player)
        {
            Player = player;
        }

        public int GetPayloadSize()
        {
            return 2048; //1024
        }

        public void WritePayload(PacketWriter writer)
        {
            PacketWriter stateBlock = new(1024); //768

            writer.SetAccessType(Packet.AccessType.BitAccess);

            Player.PlayerMovement.UpdatePersonal(Player, writer);

            if (Player.UpdateRequired)
            {
                UpdateState(Player, stateBlock, false, true);
            }
            //Update other local players
            writer.WriteBits(8, Player.LocalPlayers.Count);

            // ====== Processes all player changes in the region ========
            foreach (Player other in Player.LocalPlayers.ToArray())
            {
                if (other.Position.IsWithinDistance(Player.Position) 
                    && other.PlayerConnection.ConnectionState == ConnectionState.Authenticated 
                    && !other.Movement.Teleported)
                {
                    Player.Movement.Update(other, writer);

                    if (other.UpdateRequired)
                    {
                        UpdateState(other, stateBlock, false, false);
                    }
                }
                else
                {
                    writer.WriteBits(1, 1);
                    writer.WriteBits(2, 3);
                    Player.LocalPlayers.Remove(other);
                }
            }
            int addedLocalPlayers = 0;

            foreach (Player other in WorldHandler.World.Players.Entities)
            {
                if (Player.LocalPlayers.Count >= RegionPlayersLimit
                    || addedLocalPlayers >= NewPlayersPerCycle)
                {
                    break;
                }
                if (other == null || other == Player || Player.LocalPlayers.Contains(other)
                    || other.PlayerConnection.ConnectionState != ConnectionState.Authenticated)
                {
                    //Console.WriteLine("Getting stopped here?");
                    continue;
                }
                if (other.Position.IsWithinDistance(Player.Position))
                {
                    Player.LocalPlayers.Add(other);
                    addedLocalPlayers++;
                    // Add the other player for the player
                    writer.WriteBits(11, other.WorldIndex); // Server slot.
                    writer.WriteBits(1, 1); // Yes, an update is required.
                    writer.WriteBits(1, 1); // Discard walking queue(?)
                    // Write the relative position.
                    Position delta = Position.Delta(Player.Position, other.Position);
                    writer.WriteBits(5, delta.Y);
                    writer.WriteBits(5, delta.X);

                    UpdateState(other, stateBlock, true, false);
                }
            }
            // Append the attributes block to the main packet.
            if (stateBlock.Pointer > 0)
            {
                writer.WriteBits(11, 2047);
                writer.SetAccessType(Packet.AccessType.ByteAccess);
                writer.WriteBytes(stateBlock.Buffer, stateBlock.Pointer);
            }
            else
            {
                writer.SetAccessType(Packet.AccessType.ByteAccess);
            }
        }

        /**
         * Updates the flag-based state of a player.
         */
        private static void UpdateState(Player player, PacketWriter block, bool forceAppearance, bool noPublicChat)
        {
            // First we must calculate and write the mask.
            int mask = 0x0;

            if (player.ChatUpdateRequired && !noPublicChat)
            {
                mask |= 0x80;
            }

            if (player.AppearanceUpdateRequired || forceAppearance)
            {
                mask |= 0x10;
            }


            //now we write the mask
            if (mask >= 0x100)
            {
                mask |= 0x40;
                block.WriteShortLittleEndian(mask);
            }
            else
            {
                block.WriteByte(mask);
            }

            //Graphics
            //Animation
            //FOrced Chat
            if (player.ChatUpdateRequired && !noPublicChat)
            {
                AppendPublicChat(player, block);
            }

            //Face Entity
            //Appearance
            if (player.AppearanceUpdateRequired || forceAppearance)
            {
                AppendAppearance(player, block);
            }
            //Face Coords
            //Primary Hit
            //Secondary Hit

            // Cache and write the result
            //block.WriteBytes(result.memoryStream.GetBuffer());
            //ctx.setBuffer(forceAppearance, noPublicChat, result.getBuffer()); - no buffer writing at this time

        }


        /**
         * Appends the state of a player's public chat to a buffer.
         */
        private static void AppendPublicChat(Player player, PacketWriter writer)
        {
            if (player.Comms.ChatMessage == null)
            {
                return;
            }
            writer.WriteShortLittleEndian(((player.Comms.ChatMessage.Color & 0xff) << 8) + (player.Comms.ChatMessage.Effects & 0xff));
            writer.WriteByte((int)player.Rights);//rights
            writer.WriteByteNegated(player.Comms.ChatMessage.Text.Length);
            writer.WriteBytesReverse(player.Comms.ChatMessage.Text);
        }

        /**
         * Appends the state of a player's appearance to a buffer.
         *
         * @param player the player
         */
        private static void AppendAppearance(Player player, PacketWriter outPacket)
        {
            //PlayerAttributes attributes = player.getAttributes();
            PacketWriter block = new(MAX_APPEARANCE_BUFFER_SIZE);

            block.WriteByte(0); // Gender
            block.WriteByte(-1);//prayer icon - -1 off 6 is max
            block.WriteByte(-1); // Skull icon - -1 off 1 is max

            // Player models
            //int[] e = attributes.getEquipment();
            //int[] a = attributes.getAppearance();

            block.WriteByte(0);//hat
            //block.WriteShort(0x200 + 1139);

            block.WriteByte(0);//cape
            //block.WriteShort(0x200 + 1019);

            block.WriteByte(0);//amulet
            //block.WriteShort(0x200 + 1725);

            block.WriteByte(0);//weapon
            //block.WriteShort(0x200 + 1321);

            block.WriteShort(0x100 + player.Appearance.Chest);//chest
            //block.WriteShort(0x200 + 1103);

            block.WriteByte(0);//sheild
            //block.WriteShort(0x200 + 1189);

            block.WriteShort(0x100 + player.Appearance.Arms);//arms

            block.WriteShort(0x100 + player.Appearance.Legs);//legs
            //block.WriteShort(0x200 + 1075);

            block.WriteShort(0x100 + player.Appearance.Head);//head
            block.WriteShort(0x100 + player.Appearance.Hands);//hands
            block.WriteShort(0x100 + player.Appearance.Feet);//feet
            block.WriteShort(0x100 + player.Appearance.Beard);//beard
            block.WriteByte(player.Appearance.HairColor);//color 1?
            block.WriteByte(player.Appearance.TorsoColor);//color 2?
            block.WriteByte(player.Appearance.LegColor);//color 3?
            block.WriteByte(player.Appearance.FeetColor);//color 4?
            block.WriteByte(player.Appearance.SkinColor);//color 5?


            //// Hat.
            //if (0 > 1)
            //{
            //    block.WriteShort(0x200 + e[EquipmentHelper.EQUIPMENT_SLOT_HEAD]);
            //}
            //else
            //{
            //    block.WriteByte(0);
            //}

            //// Cape.
            //if (e[EquipmentHelper.EQUIPMENT_SLOT_CAPE] > 1)
            //{
            //    block.WriteShort(0x200 + e[EquipmentHelper.EQUIPMENT_SLOT_CAPE]);
            //}
            //else
            //{
            //    block.WriteByte(0);
            //}

            //// Amulet.
            //if (e[EquipmentHelper.EQUIPMENT_SLOT_AMULET] > 1)
            //{
            //    block.WriteShort(0x200 + e[EquipmentHelper.EQUIPMENT_SLOT_AMULET]);
            //}
            //else
            //{
            //    block.WriteByte(0);
            //}

            //// Weapon.
            //if (e[EquipmentHelper.EQUIPMENT_SLOT_WEAPON] > 1)
            //{
            //    block.WriteShort(0x200 + e[EquipmentHelper.EQUIPMENT_SLOT_WEAPON]);
            //}
            //else
            //{
            //    block.WriteByte(0);
            //}

            //// Chest.
            //if (e[EquipmentHelper.EQUIPMENT_SLOT_CHEST] > 1)
            //{
            //    block.WriteShort(0x200 + e[EquipmentHelper.EQUIPMENT_SLOT_CHEST]);
            //}
            //else
            //{
            //    block.WriteShort(0x100 + a[EquipmentHelper.APPEARANCE_SLOT_CHEST]);
            //}

            //// Shield.
            //if (e[EquipmentHelper.EQUIPMENT_SLOT_SHIELD] > 1)
            //{
            //    block.WriteShort(0x200 + e[EquipmentHelper.EQUIPMENT_SLOT_SHIELD]);
            //}
            //else
            //{
            //    block.WriteByte(0);
            //}

            //// Arms TODO: Check platebody/non-platebody.
            //if (e[EquipmentHelper.EQUIPMENT_SLOT_CHEST] > 1)
            //{
            //    block.WriteShort(0x200 + e[EquipmentHelper.EQUIPMENT_SLOT_CHEST]);
            //}
            //else
            //{
            //    block.WriteShort(0x100 + a[EquipmentHelper.APPEARANCE_SLOT_ARMS]);
            //}

            //// Legs.
            //if (e[EquipmentHelper.EQUIPMENT_SLOT_LEGS] > 1)
            //{
            //    block.WriteShort(0x200 + e[EquipmentHelper.EQUIPMENT_SLOT_LEGS]);
            //}
            //else
            //{
            //    block.WriteShort(0x100 + a[EquipmentHelper.APPEARANCE_SLOT_LEGS]);
            //}

            //// Head (with a hat already on).
            //if (EquipmentHelper.isFullHelm(e[EquipmentHelper.EQUIPMENT_SLOT_HEAD])
            //        || EquipmentHelper.isFullMask(EquipmentHelper.EQUIPMENT_SLOT_HEAD))
            //{
            //    block.WriteByte(0);
            //}
            //else
            //{
            //    block.WriteShort(0x100 + a[EquipmentHelper.APPEARANCE_SLOT_HEAD]);
            //}

            //// Hands.
            //if (e[EquipmentHelper.EQUIPMENT_SLOT_HANDS] > 1)
            //{
            //    block.WriteShort(0x200 + e[EquipmentHelper.EQUIPMENT_SLOT_HANDS]);
            //}
            //else
            //{
            //    block.WriteShort(0x100 + a[EquipmentHelper.APPEARANCE_SLOT_HANDS]);
            //}

            //// Feet.
            //if (e[EquipmentHelper.EQUIPMENT_SLOT_FEET] > 1)
            //{
            //    block.WriteShort(0x200 + e[EquipmentHelper.EQUIPMENT_SLOT_FEET]);
            //}
            //else
            //{
            //    block.WriteShort(0x100 + a[EquipmentHelper.APPEARANCE_SLOT_FEET]);
            //}

            //// Beard.
            //if (EquipmentHelper.isFullHelm(e[EquipmentHelper.EQUIPMENT_SLOT_HEAD])
            //        || EquipmentHelper.isFullMask(EquipmentHelper.EQUIPMENT_SLOT_HEAD))
            //{
            //    block.WriteByte(0);
            //}
            //else
            //{
            //    block.WriteShort(0x100 + a[EquipmentHelper.APPEARANCE_SLOT_BEARD]);
            //}

            //// Player colors
            //for (int color : attributes.getColors())
            //{
            //    block.WriteByte(color);
            //}

            // Movement animations
            block.WriteShort(0x328); // stand
            block.WriteShort(0x337); // stand turn
            block.WriteShort(0x333); // walk
            block.WriteShort(0x334); // turn 180
            block.WriteShort(0x335); // turn 90 cw
            block.WriteShort(0x336); // turn 90 ccw
            block.WriteShort(0x338); // run

            block.WriteLong(player.Credentials.UsernameAsLong);
            block.WriteByte(SkillHandler.CalculateCombatLevel(player));//combat level
            block.WriteShort(SkillHandler.GetTotalLevel(player));//total level
                                                                 //45 bytes total?

            // Append the block length and the block to the packet.
            outPacket.WriteByteNegated(block.Pointer);
            //Console.WriteLine("Block Position: " + block.Payload.Position);
            //Console.WriteLine("Block Length: " + block.Payload.Length);
            //Console.WriteLine("Buffer position before appearance: " + outPacket.Payload.Position);
            //outPacket.Payload.Write(block.Payload.ToArray(), (int)outPacket.Payload.Position, (int)4);

            //var arr1 = new byte[block.PayloadPosition];
            //Array.Copy(block.Payload.ToArray(), arr1, block.PayloadPosition);

            outPacket.WriteBytes(block.Buffer, block.Pointer);
            //Console.WriteLine("Buffer position after appearance: " + outPacket.Payload.Position);
        }

    }

}
