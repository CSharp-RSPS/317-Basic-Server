using RSPS.src.entity.player.skill;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.player
{
    public static class PlayerUpdating
    {
        /**
        * The maximum length of the update appearance block.
        */
        private static readonly int MAX_APPEARANCE_BUFFER_SIZE = 58;//full size of buffer per player
        /**
         * Regional player limit.
         */
        private static readonly int REGION_PLAYERS_LIMIT = 255;

        private static readonly int NEW_PLAYERS_PER_CYCLE = 45;

        public static void Update(World world, Player player)
        {
            try
            {
                PacketWriter outPacket = Packet.CreatePacketWriter(2048);//gets 32 players
                PacketWriter stateBlock = Packet.CreatePacketWriter(1024);

                outPacket.WriteVariableShortHeader(player.PlayerConnection.NetworkEncryptor, 81);
                outPacket.SetAccessType(Packet.AccessType.BIT_ACCESS);
                UpdateLocalPlayerMovement(player, outPacket);
                if (player.UpdateRequired)
                {
                    UpdateState(player, stateBlock, false, true);
                }

                //Update other local players
                outPacket.WriteBits(8, player.LocalPlayers.Count);

                // ====== Processes all player changes in the region ========
                for (int i = 0; i < player.LocalPlayers.Count; i++)
                {
                    Player other = player.LocalPlayers[i];
                    if (other.Position.isViewableFrom(player.Position) && other.PlayerConnection.ConnectionState == ConnectionState.Authenticated && !other.NeedsPlacement)
                    {
                        UpdateOtherPlayerMovement(other, outPacket);

                        if (other.UpdateRequired)
                        {
                            UpdateState(other, stateBlock, false, false);
                        }
                    }
                    else
                    {
                        outPacket.WriteBit(true);
                        outPacket.WriteBits(2, 3);
                        player.LocalPlayers.Remove(other);
                    }
                }
                //foreach (Player other in player.LocalPlayers.ToArray())
                //{
                //    if (other.Position.isViewableFrom(player.Position) && other.PlayerConnection.connectionState == 2 && !other.NeedsPlacement)
                //    {
                //        UpdateOtherPlayerMovement(other, outPacket);

                //        if (other.UpdateRequired)
                //        {
                //            UpdateState(other, stateBlock, false, false);
                //        }
                //    }
                //    else
                //    {
                //        outPacket.WriteBit(true);
                //        outPacket.WriteBits(2, 3);
                //        player.LocalPlayers.Remove(other);
                //    }
                //}
                int addedLocalPlayers = 0;

                //Update the local players list.
                for (int i = 0; i < world.Players.Entities.Count; i++)
                {
                    if (player.LocalPlayers.Count >= REGION_PLAYERS_LIMIT)
                    {
                        break;
                    } else if (addedLocalPlayers >= NEW_PLAYERS_PER_CYCLE)
                    {
                        break;
                    }

                    Player other = world.Players.Entities[i];
                    if (other == null || other == player || other.PlayerConnection.ConnectionState < ConnectionState.Authenticated)//so we dont add ourself to the list
                    {
                        continue;
                    }

                    //So we dont hit the client buffer of 5k - redo this keep track of players being added per cycle
                    //it's variable player adding to the seen surrounding area
/*                    if ((outPacket.PayloadPosition + stateBlock.PayloadPosition) + 320 > 5000)
                    {
                        break;
                    }*/

                    if (!player.LocalPlayers.Contains(other) && other.Position.isViewableFrom(player.Position))
                    {
                        player.LocalPlayers.Add(other);
                        addedLocalPlayers++;
                        AddPlayer(outPacket, player, other);
                        UpdateState(other, stateBlock, true, false);
                    }
                }

                //// Append the attributes block to the main packet.
                if (stateBlock.PayloadPosition > 0)
                {
                    outPacket.WriteBits(11, 2047);
                    outPacket.SetAccessType(Packet.AccessType.BYTE_ACCESS);
                    outPacket.WriteBytes(stateBlock.Payload, stateBlock.PayloadPosition);
                }
                else
                {
                    outPacket.SetAccessType(Packet.AccessType.BYTE_ACCESS);
                    //outPacket.FinishBitAccess();
                }



                // Finish the packet and send it.
                outPacket.FinishVariableShortHeader();

                player.PlayerConnection.Send(outPacket.Payload, outPacket.PayloadPosition);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                Console.Error.WriteLine("Player Updating error!");
            }
        }

        /**
         * Adds a player to the local player list of another player.
         *
         * @param out    the packet to write to
         * @param player the host player
         * @param other  the player being added
         */
        private static void AddPlayer(PacketWriter outPacket, Player player, Player other)
        {
            outPacket.WriteBits(11, other.PlayerIndex); // Server slot.
            outPacket.WriteBit(true); // Yes, an update is required.
            outPacket.WriteBit(true); // Discard walking queue(?)

            // Write the relative position.
            Position delta = Position.Delta(player.Position, other.Position);
            outPacket.WriteBits(5, delta.Y);
            outPacket.WriteBits(5, delta.X);
        }

        /**
         * Updates the movement of a player for another player (does not make use of sector 2, 3).
         */
        private static void UpdateOtherPlayerMovement(Player player, PacketWriter outPacket)
        {
            bool updateRequired = player.UpdateRequired;
            int pDir = player.PrimaryDirection;//primary direction
            int sDir = player.SecondaryDirection;//secondary direction
            UpdateMovement(outPacket, pDir, sDir, updateRequired);

            //    foreach (byte b in outPacket.Payload.ToArray())
            //    {
            //        Console.WriteLine(b);
            //    }
            //    Environment.Exit(0);
            
        }

        /**
         * Updates the flag-based state of a player.
         */
        private static void UpdateState(Player player, PacketWriter block, bool forceAppearance,
                                       bool noPublicChat)
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
                block.writeShort(mask, Packet.ByteOrder.LITTLE);
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
            writer.writeShort(((player.ChatColor & 0xff) << 8) + (player.ChatEffects & 0xff), Packet.ByteOrder.LITTLE);
            writer.WriteByte(1);//rights
            writer.WriteByte(player.ChatText.Length, Packet.ValueType.C);
            writer.WriteBytesReverse(player.ChatText);
        }

        /**
         * Appends the state of a player's appearance to a buffer.
         *
         * @param player the player
         */
        private static void AppendAppearance(Player player, PacketWriter outPacket)
        {
            //PlayerAttributes attributes = player.getAttributes();
            PacketWriter block = Packet.CreatePacketWriter(MAX_APPEARANCE_BUFFER_SIZE);

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
            outPacket.WriteByte((int)block.PayloadPosition, Packet.ValueType.C);
            //Console.WriteLine("Block Position: " + block.Payload.Position);
            //Console.WriteLine("Block Length: " + block.Payload.Length);
            //Console.WriteLine("Buffer position before appearance: " + outPacket.Payload.Position);
            //outPacket.Payload.Write(block.Payload.ToArray(), (int)outPacket.Payload.Position, (int)4);

            //var arr1 = new byte[block.PayloadPosition];
            //Array.Copy(block.Payload.ToArray(), arr1, block.PayloadPosition);

            outPacket.WriteBytes(block.Payload, block.PayloadPosition);
            //Console.WriteLine("Buffer position after appearance: " + outPacket.Payload.Position);
        }

        /**
         * Updates movement for this local player. The difference between this
         * method and the other player method is that this will make use of sector
         * 2,3 to place the player in a specific position while sector 2,3 is not
         * present in updating of other players (it simply flags local list removal
         * instead).
         */
        private static void UpdateLocalPlayerMovement(Player player, PacketWriter outPacket)
        {
            bool updateRequired = player.UpdateRequired;

            if (player.NeedsPlacement) // Do they need placement?
            {
                outPacket.WriteBit(true); // Yes, there is an update.

                int posX = player.Position.GetLocalX(player.CurrentRegion);
                int posY = player.Position.GetLocalY(player.CurrentRegion);
               // Console.WriteLine("Appending placement for: " + player.Username);//doesn't work for second player
                AppendPlacement(outPacket, posX, posY, player.Position.Z, player.ResetMovementQueue, updateRequired);
            }
            else
            { // No placement update, check for movement.
                int pDir = player.PrimaryDirection;
                int sDir = player.SecondaryDirection;
                UpdateMovement(outPacket, pDir, sDir, updateRequired);
            }
        }

        /**
         * WORKS
         **/
        private static void UpdateMovement(PacketWriter outPacket, int pDir, int sDir, bool updateRequired)
        {
            if (pDir != -1)//// If they moved.
            {
                outPacket.WriteBit(true); // Yes, there is an update.

                if (sDir != -1) // If they ran.
                {
                    AppendRun(outPacket, pDir, sDir, updateRequired);
                }
                else
                { // Movement but no running - they walked.
                    AppendWalk(outPacket, pDir, updateRequired);
                }
            }
            else//// No movement.
            {
                if (updateRequired) // Does the state need to be updated?
                {
                    outPacket.WriteBit(true); // Yes, there is an update.
                    AppendStand(outPacket);
                }
                else
                { // No update whatsoever.
                    outPacket.WriteBit(false);
                }
            }
        }


        /**
         * Appends the player placement version of the movement section of the update packet (sector 2,3).
         * Note that by others this was previously called the "teleport update".
         *
         * @param localX               the local X coordinate
         * @param localY               the local Y coordinate
         * @param z                    the Z coordinate
         * @param discardMovementQueue whether or not the client should discard the movement queue
         * @param attributesUpdate     whether or not a plater attributes update is required
         */
        private static void AppendPlacement(PacketWriter outPacket, int localX, int localY, int z,
                                            bool discardMovementQueue, bool attributesUpdate)
        {
            outPacket.WriteBits(2, 3); // 3 - placement.
            // Append the actual sector.
            outPacket.WriteBits(2, z);
            outPacket.WriteBit(discardMovementQueue);
            outPacket.WriteBit(attributesUpdate);
            //Console.WriteLine("LocalY: {0}", localY);
            outPacket.WriteBits(7, localY);
            outPacket.WriteBits(7, localX);
            //foreach(var b in outPacket.Payload.ToArray())
            //{
            //    Console.WriteLine(b);
            //}
            //System.Environment.Exit(0);
        }

        /**
         * Appends the walk version of the movement section of the update packet
         * (sector 2,1).
         *
         * @param out              the buffer to append to
         * @param direction        the walking direction
         * @param attributesUpdate whether or not a player attributes update is required
         */
        private static void AppendWalk(PacketWriter outPacket, int direction, bool attributesUpdate)
        {
            outPacket.WriteBits(2, 1); // 1 - walking.

            // Append the actual sector.
            outPacket.WriteBits(3, direction);
            outPacket.WriteBit(attributesUpdate);
        }

        /**
         * Appends the walk version of the movement section of the update packet (sector 2,2).
         *
         * @param direction        the walking direction
         * @param direction2       the running direction
         * @param attributesUpdate whether or not a player attributes update is required
         */
        private static void AppendRun(PacketWriter outPacket, int direction, int direction2, bool attributesUpdate)
        {
            outPacket.WriteBits(2, 2); // 2 - running.

            // Append the actual sector.
            outPacket.WriteBits(3, direction);
            outPacket.WriteBits(3, direction2);
            outPacket.WriteBit(attributesUpdate);
        }

        /**
         * Appends the stand version of the movement section of the update packet
         * (sector 2,0). Appending this (instead of just a zero bit) automatically
         * assumes that there is a required attribute update afterwards.
         */
        private static void AppendStand(PacketWriter outPacket)
        {
            outPacket.WriteBits(2, 0); // 0 - no movement.
        }

    }
}
