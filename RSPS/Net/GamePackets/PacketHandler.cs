using RSPS.Entities.Mobiles.Players;
using RSPS.Net.Connections;
using RSPS.Net.GamePackets.Receive;
using RSPS.Net.GamePackets.Receive.Impl;
using RSPS.Net.GamePackets.Send;
using RSPS.Util.Attributes;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;

namespace RSPS.Net.GamePackets
{
    /// <summary>
    /// Handles packet related operations
    /// </summary>
    public static class PacketHandler
    {

        /// <summary>
        /// Holds the receivable packets for players
        /// </summary>
        private static readonly Dictionary<int, IReceivePacket> ReceivablePackets = new();


        static PacketHandler() {
            ReceivablePackets.Add(0, new ReceiveIdle()); //Sent when there are no actions being performed by the player for this cycle.
            ReceivablePackets.Add(3, new ReceiveFocusChange()); //Sent when the game client window goes in and out of focus.
            ReceivablePackets.Add(4, new ReceiveChat()); //Sent when the player enters a chat message.
            ReceivablePackets.Add(14, new ReceiveItemOnPlayer()); //Sent when a player uses an item on another player.
            ReceivablePackets.Add(16, new ReceiveItemOption2()); //Sent when a player uses an item. This is an alternate item option.
            ReceivablePackets.Add(17, new ReceiveNpcOption2()); //Sent when a player clicks the second option of an NPC.
            ReceivablePackets.Add(18, new ReceiveNpcOption4()); //Sent when a player clicks the fourth option of an NPC.
            ReceivablePackets.Add(21, new ReceiveNpcOption3()); //Sent when a player clicks the third option of an NPC.
            ReceivablePackets.Add(25, new ReceiveItemOnFloor()); //Sent when a player uses an item on another item thats on the floor.
            ReceivablePackets.Add(35, new ReceiveMagicOnObject()); //Send when a player uses magic on an object.
            ReceivablePackets.Add(36, new ReceiveValidateWalking()); //Send to validate walking.
            ReceivablePackets.Add(39, new ReceiveFollow()); //Sent when a player clicks the follow option on another player.
            ReceivablePackets.Add(40, new ReceiveDialogue()); //Send when a player continues a dialogue.
            ReceivablePackets.Add(41, new ReceiveEquipItem()); //Sent when a player equips an item.
            ReceivablePackets.Add(43, new InterfaceItemOption3()); //Sent when a player banks 10 of a certain item.
            ReceivablePackets.Add(45, new ReceiveFlaggedAccount()); //Sent when a players account is flagged.
            ReceivablePackets.Add(53, new ReceiveItemOnItem()); //Sent when a player uses an item with another item.
            ReceivablePackets.Add(57, new ReceiveItemOnNpc()); //Sent when a player uses an item on an NPC.
            ReceivablePackets.Add(60, new ReceiveTypingOntoInterface()); //Sent while typing onto an interface
            ReceivablePackets.Add(70, new ReceiveObjectOption3()); //Sent when the player clicks the third action available for an object.
            ReceivablePackets.Add(72, new ReceiveAttackNpc()); //Sent when a player attacks an NPC.
            ReceivablePackets.Add(73, new ReceiveAttackPlayer()); //Sent when a player selects the attack option on another player.
            ReceivablePackets.Add(74, new ReceiveRemoveIgnore()); //Sent when a player removes a player from their ignore list.
            ReceivablePackets.Add(75, new ReceiveItemOption3()); //Send when a player clicks the third option of an item.
            ReceivablePackets.Add(79, new ReceiveLightItem()); //Sent when a player attempts to light logs on fire.
            ReceivablePackets.Add(85, new ReceiveValidateNpcOption4()); //Sent to validate npc option 4. (client action 478)
            ReceivablePackets.Add(86, new ReceiveCameraMovement()); //Sent when the player moves the camera.
            ReceivablePackets.Add(87, new ReceiveDropItem()); //Sent when a player wants to drop an item onto the ground.
            ReceivablePackets.Add(95, new ReceivePrivacyOptions()); //Sent when a player changes their privacy options (i.e. public chat).
            ReceivablePackets.Add(98, new ReceiveWalkOnCommand()); //Sent when the player should walk somewhere according to a certain action performed, such as clicking an object.
            ReceivablePackets.Add(101, new ReceiveDesignScreen()); //Sent when a player is choosing their character design options.
            ReceivablePackets.Add(103, new ReceivePlayerCommand()); //Sent when the player enters a command in the chat box (e.g. "::command")
            ReceivablePackets.Add(117, new InterfaceItemOption2()); //Sent when a player banks 5 of a certain item.
            ReceivablePackets.Add(121, new ReceiveLoadingFinished()); //Sent when the client finishes loading a map region.
            ReceivablePackets.Add(122, new ReceiveItemOption1()); //Sent when the player clicks the first option of an item, such as "Bury" for bones.
            ReceivablePackets.Add(126, new ReceivePrivateMessage()); //Sent when a player sends a private message to another player.
            ReceivablePackets.Add(128, new ReceiveAcceptChallenge()); //Sent when a player accepts another players duel request.
            ReceivablePackets.Add(129, new InterfaceItemOption4()); //Sent when a player banks all of a certain item that they have in their inventory.
            ReceivablePackets.Add(130, new ReceiveCloseWindow()); //Sent when a player presses the close, exit or cancel button on an interface.
            ReceivablePackets.Add(131, new ReceiveMagicOnNpc()); //Sent when a player uses magic on an npc.
            ReceivablePackets.Add(132, new ReceiveObjectOption1()); //Sent when the player clicks the first option of an object, such as "Cut" for trees.
            ReceivablePackets.Add(133, new ReceiveAddIgnore()); //Sent when a player adds a player to their ignore list.
            ReceivablePackets.Add(135, new ReceiveBankXItemsPt1()); //Sent when a player requests to bank an X amount of items.
            ReceivablePackets.Add(136, new ReceiveValidatePlayerOption1()); //Send with client action 561, 6 has to do with player option 1
            ReceivablePackets.Add(139, new ReceiveTradeRequest()); //Sent when a player Requests a trade from another player. (e.g. "Sending Trade Request...")
            ReceivablePackets.Add(145, new ReceiveInterfaceItemOption1()); //Sent when a player unequips an item.
            ReceivablePackets.Add(152, new ReceiveValidateNpcOption3()); //Send to validate npc option 3 (client action 965)
            ReceivablePackets.Add(153, new ReceivePlayerOption2()); //Sent when a moderator or administrator selects the second option of a player.
            ReceivablePackets.Add(155, new ReceiveNpcOption1()); //Sent when a player clicks first option of an NPC, such as "Talk."
            ReceivablePackets.Add(164, new ReceiveRegularWalk()); //Sent when the player walks regularly.
            ReceivablePackets.Add(181, new ReceiveMagicOnGroundItem()); //Send when a player uses a spell on a ground item.
            ReceivablePackets.Add(183, new ReceiveValidateObjectOption4()); //Validates clicking object option 4
            ReceivablePackets.Add(185, new ReceiveButtonClick()); //Sent when a player clicks an in-game button.
            ReceivablePackets.Add(188, new ReceiveAddFriend()); //Sent when a player adds a friend to their friend list.
            ReceivablePackets.Add(189, new ReceiveValidatePlayerOption2()); //Validates player option 2
            ReceivablePackets.Add(192, new ReceiveItemOnObject()); //Sent when a a player uses an item on an object.
            ReceivablePackets.Add(200, new ReceiveValidateBankingOptions()); //Validates banking options
            ReceivablePackets.Add(202, new ReceiveIdleLogout()); //Sent when the player has become idle and should be logged out.
            ReceivablePackets.Add(208, new ReceiveBankXItemsPt2()); //Sent when a player enters an X amount of items they want to bank.
            ReceivablePackets.Add(210, new ReceiveRegionChange()); //Sent when a player enters a new map region.
            ReceivablePackets.Add(214, new ReceiveMoveItem()); //Sent when a player moves an item from one slot to another.
            ReceivablePackets.Add(215, new ReceiveRemoveFriend()); //Sent when a player removes a friend from their friend list.
            ReceivablePackets.Add(218, new ReceiveReportPlayer()); //Sent when a player reports another player.
            ReceivablePackets.Add(228, new ReceiveObjectOption4()); //Sent when a player uses the 4th option of an object.
            ReceivablePackets.Add(230, new ReceiveValidateNpcOption2()); //Validates NPC option 2
            ReceivablePackets.Add(234, new ReceiveGroundItemOption1()); //Send when a player uses the 2nd option of an object.
            ReceivablePackets.Add(236, new ReceivePickupGroundItem()); //Sent when the player picks up an item from the ground.
            ReceivablePackets.Add(237, new ReceiveMagicOnItems()); //Sent when a player casts magic on the items in their inventory.
            ReceivablePackets.Add(241, new ReceiveMouseClick()); //Sent when the player clicks somewhere on the game screen.
            ReceivablePackets.Add(246, new ReceiveUnknownAntiCheat()); //Not sure
            ReceivablePackets.Add(248, new ReceiveMapWalk()); //Sent when the player walks using the map. Has 14 additional (assumed to be anticheat) bytes added to the end of it that are ignored.
            ReceivablePackets.Add(249, new ReceiveMagicOnPlayer()); //Sent when a player attempts to cast magic on another player.
            ReceivablePackets.Add(252, new ReceiveObjectOption2()); //Sent when the player clicks the second option available for an object.
            ReceivablePackets.Add(253, new ReceiveGroundItemOption1()); //Sent when the player clicks the first option for a ground item
        }

        /// <summary>
        /// Handles a received packet for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="packetReader">The packet reader</param>
        public static void HandlePacket(Player player, PacketReader packetReader)
        {
            if (!ReceivablePackets.ContainsKey(packetReader.Opcode)) {
                if (packetReader.Opcode == 77 || packetReader.Opcode == 165
                    || packetReader.Opcode == 226 || packetReader.Opcode == 246)
                { // Ignore old security related packets without purpose, that are not useful anymore
                    return;
                }
                Console.Error.WriteLine("Unknown packet {0} (Length: {1})", packetReader.Opcode, packetReader.PayloadSize);
                return;
            }
            IReceivePacket receivePacket = ReceivablePackets[packetReader.Opcode];
            PacketInfoAttribute? packetInfo = receivePacket.GetPacketInfo();

            if (packetInfo == null)
            {
                Console.Error.WriteLine("No packet info present on packet receiver {0} [Opcode: {1}][Size: {2}]",
                    receivePacket.GetType().Name, packetReader.Opcode, packetReader.PayloadSize);
                return;
            }
            receivePacket.ReceivePacket(player, packetReader);

            if (packetReader.Opcode != 0)
            {
                Debug.WriteLine("Handled packet [Opcode: {0}][Payload size: {1}]", packetReader.Opcode, packetReader.PayloadSize);
            }
        }

        /// <summary>
        /// Attempts to send a packet without payload to a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="packetDef">The packet definition</param>
        public static void SendPacket(Player player, SendPacketDefinition packetDef)
        {
            if (player.PlayerConnection == null)
            {
                Debug.WriteLine("Unable to send packet for player {0} because the player has no active connection", player.Credentials.Username);
                return;
            }
            PacketInfoAttribute? packetInfo = packetDef.GetPacketInfo();

            if (packetInfo == null)
            {
                Debug.WriteLine("No packet info attribute present on packet definition: {0}", packetDef.ToString());
                return;
            }

            if (packetInfo.HasPayload)
            {
                Debug.WriteLine("Packet should have a payload but it used here like it has not: {0}", packetDef.ToString());
                return;
            }
            SendPacket(player.PlayerConnection, packetDef, null);
        }

        /// <summary>
        /// Attempts to send a packet with payload to a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="packetPayloadBuilder">The packet payload builder</param>
        public static void SendPacket(Player player, IPacketPayloadBuilder packetPayloadBuilder)
        {
            PacketDefAttribute? packetDefAttr = packetPayloadBuilder.GetPacketDef();

            if (packetDefAttr == null)
            {
                Debug.WriteLine("No packet definition attribute present on packet payload builder: {0}", packetPayloadBuilder.GetType().Name);
                return;
            }
            SendPacket(player.PlayerConnection, packetDefAttr.PacketDef, packetPayloadBuilder);
        }

        /// <summary>
        /// Attempts to send a packet to the client
        /// </summary>
        /// <param name="connection">The client connection</param>
        /// <param name="packetDef">The packet definition</param>
        /// <param name="packetPayloadBuilder">The packet payload builder</param>
        private static void SendPacket(Connection connection, SendPacketDefinition packetDef, IPacketPayloadBuilder? packetPayloadBuilder = null)
        {
            if (connection.NetworkEncryptor == null)
            {
                Console.Error.WriteLine("Unable to send packet, no encryptor present");
                connection.Dispose();
                return;
            }
            // Retrieve the packet information for a packet definition
            PacketInfoAttribute? packetInfo = packetDef.GetPacketInfo();

            if (packetInfo == null)
            { // No packet info attribute found
                throw new ArgumentNullException(nameof(packetDef));
            }
            // Determine the size of the packet, a packet without payload is a fixed packet requiring 1 byte for the opcode
            int packetSize = !packetInfo.HasPayload ? 1 : -1;

            if (packetInfo.HasPayload)
            {
                if (packetPayloadBuilder == null)
                { // A packet payload builder should be presented in case of a packet with payload
                    throw new ArgumentNullException(nameof(packetPayloadBuilder));
                }
                if (!packetInfo.HasVariableSize)
                { // Fixed payload size + add an additional byte for the opcode
                    packetSize = packetInfo.PayloadSize + 1;
                }
                else
                {
                    if (packetPayloadBuilder is not IPacketVariablePayloadBuilder)
                    { // A packet variable payload builder should be presented in case of a variable packet size
                        throw new ArgumentException("Packets with a variable size should be implementing the variable packet payload builder", nameof(packetPayloadBuilder));
                    }
                    // Additonally to the payload size,
                    // 1 byte for defining the payload size within the packet
                    // and 1 or 2 bytes for the opcode depending on it being a byte or a short
                    packetSize = (packetInfo.HeaderType == PacketHeaderType.VariableByte ? 2 : 3) 
                        + ((IPacketVariablePayloadBuilder)packetPayloadBuilder).GetPayloadSize();
                    //packetSize = ((IPacketVariablePayloadBuilder)packetPayloadBuilder).GetPayloadSize();
                }
            }
            PacketWriter packetWriter = new(packetSize);
            // Write the packet header
            packetWriter.WriteHeader(packetInfo.HeaderType, connection.NetworkEncryptor, packetInfo.Opcode);

            if (packetInfo.HasPayload && packetPayloadBuilder != null)
            { // Write the packet payload
                packetPayloadBuilder.WritePayload(packetWriter);
            }
            if (packetInfo.HasVariableSize)
            { // Finish the packet header in case of variables packets
                packetWriter.FinishHeader();
            }
            // Dispatch the packet to the client
            connection.Send(packetWriter.Buffer, packetWriter.Pointer);

            if (packetInfo.Opcode != 81 && packetInfo.Opcode != 65)
            {
                Debug.WriteLine("Packet ({0}) [Opcode: {1}][Size: {2}] dispatched to client",
                Enum.GetName(typeof(SendPacketDefinition), packetDef), packetInfo.Opcode, packetSize);
            }
        }

        /// <summary>
        /// Retrieves the packet info attribute on a packet definition value
        /// </summary>
        /// <param name="packetDef">The packet definition value</param>
        /// <returns>The packet info attribute</returns>
        public static PacketInfoAttribute? GetPacketInfo(this SendPacketDefinition packetDef)
        {
            FieldInfo? fieldInfo = packetDef.GetType().GetField(packetDef.ToString());
            return fieldInfo == null 
                ? null 
                : fieldInfo.GetCustomAttributes(typeof(PacketInfoAttribute), false).FirstOrDefault() as PacketInfoAttribute;
        }

        /// <summary>
        /// Retrieves the packet definition attribute on a receive packet handler
        /// </summary>
        /// <param name="receivePacket">The receive packet handler</param>
        /// <returns>The packet definition attribute</returns>
        public static PacketInfoAttribute? GetPacketInfo(this IReceivePacket receivePacket)
        {
            Attribute? attr = Attribute.GetCustomAttribute(receivePacket.GetType(), typeof(PacketInfoAttribute));
            return attr == null ? null : (PacketInfoAttribute)attr;
        }

        /// <summary>
        /// Retrieves the packet definition attribute on a packet payload builder
        /// </summary>
        /// <param name="packetPayloadBuilder">The packet payload builder</param>
        /// <returns>The packet definition attribute</returns>
        public static PacketDefAttribute? GetPacketDef(this IPacketPayloadBuilder packetPayloadBuilder)
        {
            Attribute? attr = Attribute.GetCustomAttribute(packetPayloadBuilder.GetType(), typeof(PacketDefAttribute));
            return attr == null ? null : (PacketDefAttribute)attr;
        }

    }
}
