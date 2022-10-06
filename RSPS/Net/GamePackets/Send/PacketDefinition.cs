using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send
{
    /// <summary>
    /// Holds the definitions for the packets that can be sent to clients
    /// </summary>
    public enum PacketDefinition
    {

        /// <summary>
        /// Resets all animations in the immediate area.
        /// </summary>
        [PacketInfo(1, 0)]
        AnimationReset = 1,

        /// <summary>
        /// Draw a graphic at a given x/y position after a delay.
        /// </summary>
        [PacketInfo(4, 6)]
        DrawGraphicAtPosition = 4,

        /// <summary>
        /// Draw a given model on a given interface.
        /// </summary>
        [PacketInfo(8, 4)]
        SetInterfaceModel = 8,

        /// <summary>
        /// Causes a sidebar icon to start flashing.
        /// </summary>
        [PacketInfo(24, 1)]
        FlashSidebar = 24,

        /// <summary>
        /// Displays the "Input amount" interface.
        /// </summary>
        [PacketInfo(27, 0)]
        InputAmount = 27,

        /// <summary>
        /// Draw a collection of items on an interface.
        /// </summary>
        [PacketInfo(34, PacketHeaderType.VariableShort)]
        ItemOnInterface = 34,

        /// <summary>
        /// Causes the camera to shake.
        /// </summary>
        [PacketInfo(35, 4)]
        CameraShake = 35,

        /// <summary>
        /// Forcefully changes a client's setting's value. Also changes the default value for that setting.
        /// </summary>
        [PacketInfo(36, 3)]
        ForceClientSetting = 36,

        /// <summary>
        /// Place an item stack on the ground.
        /// </summary>
        [PacketInfo(44, 5)]
        SendGroundItem = 44,

        /// <summary>
        /// Sends a friend to the friend list.
        /// </summary>
        [PacketInfo(50, 9)]
        SendAddFriend = 50,

        /// <summary>
        /// Draw a collection of items on an interface.
        /// </summary>
        [PacketInfo(53, PacketHeaderType.VariableShort)]
        DrawItemsOnInterface = 53,

        /// <summary>
        /// Begin processing position related packets.
        /// </summary>
        [PacketInfo(60, PacketHeaderType.VariableShort)]
        BeginProcessingPositionRelatedPackets = 60,

        /// <summary>
        /// Shows the player that they are in a multi-combat zone.
        /// </summary>
        [PacketInfo(61, 1)]
        ShowMultiCombat = 61,

        /// <summary>
        /// Delete ground item.
        /// </summary>
        [PacketInfo(64, 2)]
        DeleteGroundItem = 64,

        /// <summary>
        /// NPC updating
        /// </summary>
        [PacketInfo(65, PacketHeaderType.VariableShort)]
        NPCUpdating = 65,

        /// <summary>
        /// Resets the button state for all buttons.
        /// </summary>
        [PacketInfo(68, 0)]
        ResetButtonState = 68,

        /// <summary>
        /// Sets the offset for drawing of an interface.
        /// </summary>
        [PacketInfo(70, 6)]
        InterfaceOffset = 70,

        /// <summary>
        /// Assigns an interface to one of the tabs in the game sidebar.
        /// </summary>
        [PacketInfo(71, 3)]
        SendSidebarInterface = 71,

        /// <summary>
        /// Clears an interface's inventory.
        /// </summary>
        [PacketInfo(72, 2)]
        ClearInventory = 72,

        /// <summary>
        /// Loads a new map region.
        /// </summary>
        [PacketInfo(73, 4)]
        LoadMapRegion = 73,

        /// <summary>
        /// Starts playing a song.
        /// </summary>
        [PacketInfo(74, 4)]
        PlaySong = 74,

        /// <summary>
        /// Place the head of an NPC on an interface
        /// </summary>
        [PacketInfo(75, 4)]
        NPCHeadOnInterface = 75,

        /// <summary>
        /// Resets the players' destination.
        /// </summary>
        [PacketInfo(78, 0)]
        ResetDestination = 78,

        /// <summary>
        /// Sets the scrollbar position of an interface.
        /// </summary>
        [PacketInfo(79, 4)]
        ScrollPosition = 79,

        /// <summary>
        /// Begins the player update procedure
        /// </summary>
        [PacketInfo(81, PacketHeaderType.VariableShort)]
        BeginPlayerUpdating = 81,

        /// <summary>
        /// Edit ground item amount
        /// </summary>
        [PacketInfo(84, 7)]
        EditGroundItemAmount = 84,

        /// <summary>
        /// Set local player coordinates
        /// </summary>
        [PacketInfo(85, 2)]
        SetLocalPlayerCoordinates = 85,

        /// <summary>
        /// Forcefully changes a client's setting's value. Also changes the default value for that setting.
        /// </summary>
        [PacketInfo(87, 7)]
        ForceClientSetting2 = 87,

        /// <summary>
        /// Displays a normal interface.
        /// </summary>
        [PacketInfo(97, 2)]
        ShowInterface = 97,

        /// <summary>
        /// Sets the mini map's state.
        /// </summary>
        [PacketInfo(99, 1)]
        MinimapState = 99,

        /// <summary>
        /// Sends an object removal request to the client.
        /// </summary>
        [PacketInfo(101, 3)]
        ObjectRemoval = 101,

        /// <summary>
        /// Adds a player option to the right click menu of player(s).
        /// </summary>
        [PacketInfo(104, PacketHeaderType.VariableByte)]
        PlayerOption = 104,

        /// <summary>
        /// Play sound in location.
        /// </summary>
        [PacketInfo(105, 4)]
        PlaySoundInLocation = 105,

        /// <summary>
        /// Draws an interface over the tab area.
        /// </summary>
        [PacketInfo(106, 1)]
        InterfaceOverTab = 106,

        /// <summary>
        /// Resets the camera position.
        /// </summary>
        [PacketInfo(107, 0)]
        ResetCamera = 107,

        /// <summary>
        /// Disconnects the client from the server.
        /// </summary>
        [PacketInfo(109, 0)]
        Logout = 109,

        /// <summary>
        /// Sends the players run energy level.
        /// </summary>
        [PacketInfo(110, 1)]
        RunEnergy = 110,

        /// <summary>
        /// Sends how many seconds until a 'System Update.'
        /// </summary>
        [PacketInfo(114, 2)]
        SystemUpdate = 114,

        /// <summary>
        /// Creates a projectile.
        /// </summary>
        [PacketInfo(117, 15)]
        CreateProjectile = 117,

        /// <summary>
        /// Queues a song to be played next.
        /// </summary>
        [PacketInfo(121, 4)]
        SongQueue = 121,

        /// <summary>
        /// Changes the color of an interface.
        /// </summary>
        [PacketInfo(122, 4)]
        InterfaceColor = 122,

        /// <summary>
        /// Attaches text to an interface.
        /// </summary>
        [PacketInfo(126, PacketHeaderType.VariableShort)]
        SetInterfaceText = 126,

        /// <summary>
        /// Sends a skill level to the client.
        /// </summary>
        [PacketInfo(134, 6)]
        SkillLevel = 134,

        /// <summary>
        /// Show inventory interface
        /// </summary>
        [PacketInfo(142, 2)]
        ShowInventoryInterface = 142,

        /// <summary>
        /// Player to object transformation
        /// </summary>
        [PacketInfo(147, 14)]
        PlayerToObjectTransformation = 147,

        /// <summary>
        /// Sends an object spawn request to the client.
        /// </summary>
        [PacketInfo(151, 5)]
        ObjectSpawn = 151,

        /// <summary>
        /// Remove non-specified ground items??????
        /// </summary>
        [PacketInfo(156, 3)]
        RemoveNonSpecifiedGroundItems = 156,

        /// <summary>
        /// Shows an interface in the chat box??????
        /// </summary>
        [PacketInfo(160, 4)]
        SpawnAnimatedObject = 160,

        /// <summary>
        /// Shows an interface in the chat box.
        /// </summary>
        [PacketInfo(164, 2)]
        ChatInterface = 164,

        /// <summary>
        /// Spin camera
        /// </summary>
        [PacketInfo(166, 6)]
        SpinCamera = 166,

        /// <summary>
        /// Sets an interface to be hidden until hovered over.
        /// </summary>
        [PacketInfo(171, 3)]
        HiddenInterface = 171,

        /// <summary>
        /// Sets what audio/sound is to play at a certain time.
        /// </summary>
        [PacketInfo(174, 5)]
        Audio = 174,

        /// <summary>
        /// Displays the welcome screen.
        /// </summary>
        [PacketInfo(176, 10)]
        OpenWelcomeScreen = 176,

        /// <summary>
        /// Set cutscene camera
        /// </summary>
        [PacketInfo(177, 6)]
        SetCutsceneCamera = 177,

        /// <summary>
        /// Sends the players head model to an interface
        /// </summary>
        [PacketInfo(185, 2)]
        PlayerHeadToInterface = 185,

        /// <summary>
        /// Displays the "Enter name" interface.
        /// </summary>
        [PacketInfo(187, 0)]
        EnterName = 187,

        /// <summary>
        /// Sends a private message to another player.
        /// </summary>
        [PacketInfo(196, PacketHeaderType.VariableShort)]
        SendPrivateMessage = 196,

        /// <summary>
        /// Sets an interface's model animation.
        /// </summary>
        [PacketInfo(200, 4)]
        InterfaceAnimation = 200,

        /// <summary>
        /// Sends the chat privacy settings.
        /// </summary>
        [PacketInfo(206, 3)]
        ChatSettings = 206,

        /// <summary>
        /// Displays an interface in walkable mode.
        /// </summary>
        [PacketInfo(208, 2)]
        WalkableInterface = 208,

        /// <summary>
        /// Sends a ignored player to the ignore list.
        /// </summary>
        [PacketInfo(214, PacketHeaderType.VariableShort)]
        SendAddIgnore = 214,

        /// <summary>
        /// Spawn ground item for all except specified player
        /// </summary>
        [PacketInfo(215, 7)]
        SpawnGroundItemForAllExceptSpecifiedPlayer = 215,

        /// <summary>
        /// Opens an interface over the chatbox.
        /// </summary>
        [PacketInfo(218, 2)]
        OpenChatboxInterface = 218,

        /// <summary>
        /// Clears the screen of all open interfaces.
        /// </summary>
        [PacketInfo(219, 0)]
        ClearScreen = 219,

        /// <summary>
        /// Friends list load status.
        /// </summary>
        [PacketInfo(221, 1)]
        FriendsListStatus = 221,

        /// <summary>
        /// Sets an interface's model rotation and zoom
        /// </summary>
        [PacketInfo(230, 8)]
        InterfaceModelRotation = 230,

        /// <summary>
        /// Sends the players weight amount.
        /// </summary>
        [PacketInfo(240, 2)]
        Weight = 240,

        /// <summary>
        /// Constructs a dynamic map region using a palette of 8*8 tiles.
        /// </summary>
        [PacketInfo(241, PacketHeaderType.VariableShort)]
        ConstructMapRegion = 241,

        /// <summary>
        /// Displays an item model inside an interface.
        /// </summary>
        [PacketInfo(246, 6)]
        InterfaceItem = 246,

        /// <summary>
        /// Displays an interface over the sidebar area.
        /// </summary>
        [PacketInfo(248, 4)]
        InventoryOverlay = 248,

        /// <summary>
        /// Sends the player's membership status and their current index on the server's player list.
        /// </summary>
        [PacketInfo(249, 3)]
        InitializePlayer = 249,

        /// <summary>
        /// Sends a server message (e.g. 'Welcome to RuneScape') or trade/duel request.
        /// </summary>
        [PacketInfo(253, PacketHeaderType.VariableByte)]
        SendMessage = 253,

        /// <summary>
        /// Displays a hint icon.
        /// </summary>
        [PacketInfo(254, PacketHeaderType.VariableByte)]
        DisplayHintIcon = 254

    }
}
