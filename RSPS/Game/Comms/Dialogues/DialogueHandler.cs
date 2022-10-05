using RSPS.Data;
using RSPS.Entities.Mobiles.Npcs;
using RSPS.Entities.Mobiles.Npcs.Definitions;
using RSPS.Entities.Mobiles.Players;
using RSPS.Game.UI;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Comms.Dialogues
{
    /// <summary>
    /// Handles dialogue related operations
    /// </summary>
    public static class DialogueHandler
    {

        /// <summary>
        /// The dialogue interface ID's based on the number of text lines, for option dialogues
        /// </summary>
        private static readonly int[] OptionDialougeInterfaceIds = new int[] { -1, 14443, 2469, 8207, 8219 };

        /// <summary>
        /// The dialogue interface ID's based on the number of text lines, for NPC dialogues
        /// </summary>
        private static readonly int[] NpcDialougeInterfaceIds = new int[] { 4882, 4887, 4893, 4900 };

        /// <summary>
        /// The dialogue interface ID's based on the number of text lines, for player dialogues
        /// </summary>
        private static readonly int[] PlayerDialougeInterfaceIds = new int[] { 968, 973, 979, 986 };

        /// <summary>
        /// Holds the available dialogues
        /// </summary>
        private static readonly Dictionary<int, Dialogue> Dialogues = new();


        static DialogueHandler()
        {
            Dialogues.Add(999, new Dialogue(999, -1, DialogueExpression.Uninterested, new string[] { 
                "test"
            }));

            JsonUtil.DataImport<Dialogue>("./Resources/dialogues.json", (dias) => dias.ForEach(dia => Dialogues.Add(dia.Id, dia)));
        }

        /// <summary>
        /// Indicates a player picked an option in an option dialogue
        /// </summary>
        /// <param name="option">The picked option</param>
        public static void PickOption(Player player, int option)
        {
            if (player.NonPersistentVars.CurrentDialogue == null
                || player.NonPersistentVars.CurrentDialogue.NextId == -1)
            {
                player.NonPersistentVars.CurrentDialogue = null;
                PacketHandler.SendPacket(player, PacketDefinition.ClearScreen);
                return;
            }
            // TODO: Different dia or executes based on option

            if (player.NonPersistentVars.CurrentDialogue is CustomDialogue customDia)
            {
                customDia.Execute(player);
            }
            // TODO
        }

        /// <summary>
        /// Continues an ongoing dialogue for a player
        /// </summary>
        /// <param name="player">The player</param>
        public static void ContinueDialogue(Player player)
        {
            PacketHandler.SendPacket(player, PacketDefinition.ClearScreen);

            if (player.NonPersistentVars.CurrentDialogue == null)
            {
                return;
            }
            if (player.NonPersistentVars.CurrentDialogue is CustomDialogue customDia)
            {
                customDia.Execute(player);
            }
            if (player.NonPersistentVars.CurrentDialogue.NextId == -1)
            {
                player.NonPersistentVars.CurrentDialogue = null;
                return;
            }
            SendDialogue(player, player.NonPersistentVars.CurrentDialogue.NextId);
        }

        /// <summary>
        /// Sends a dialogue to the chatbox of a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="id">The dialogue identifier</param>
        /// <param name="npcId">The npc ID when applicable</param>
        /// <exception cref="Exception"></exception>
        public static void SendDialogue(Player player, int id, int? npcId = null)
        {
            Dialogue dia = Dialogues.ContainsKey(id) ? Dialogues[id] : Dialogues[0];
            player.NonPersistentVars.CurrentDialogue = dia;

            int[] diaInterfaceIds;

            if (dia.Expression == null)
            { // Option dialogue
                diaInterfaceIds = OptionDialougeInterfaceIds;
            }
            else if (npcId != null)
            { // NPC dialogue
                diaInterfaceIds = NpcDialougeInterfaceIds;
            }
            else
            { // Player dialogue
                diaInterfaceIds = PlayerDialougeInterfaceIds;
            }
            if (dia.Lines.Length == 0 || dia.Lines.Length > diaInterfaceIds.Length)
            {
                throw new Exception("Invalid dialogue encountered (ID: " + id + ")");
            }
            int interfaceId = diaInterfaceIds[dia.Lines.Length - 1];

            PacketHandler.SendPacket(player, new SendChatInterface(interfaceId));

            if (dia.Expression == null)
            { // Option dialogue
                
            }
            else if (npcId != null)
            { // NPC dialogue
                NpcDefinition? npcDef = NpcManager.GetDef(npcId.Value);

                PacketHandler.SendPacket(player, new SendNpcHeadOnInterface(npcId.Value, ++interfaceId));
                PacketHandler.SendPacket(player, new SendInterfaceAnimation(interfaceId,
                    EnumUtil.GetAttributeOfType<FacialExpressionAttribute>(dia.Expression).Value));
                PacketHandler.SendPacket(player, new SendSetInterfaceText(++interfaceId, npcDef == null ? "Undefined" : npcDef.Name));
            }
            else
            { // Player dialogue
                PacketHandler.SendPacket(player, new SendPlayerHeadToInterface(++interfaceId));
                PacketHandler.SendPacket(player, new SendInterfaceAnimation(interfaceId,
                    EnumUtil.GetAttributeOfType<FacialExpressionAttribute>(dia.Expression).Value));
                PacketHandler.SendPacket(player, new SendSetInterfaceText(++interfaceId, player.Credentials.Username));
            }
            for (int i = 0; i < dia.Lines.Length; i++)
            {
                PacketHandler.SendPacket(player, new SendSetInterfaceText(++interfaceId, dia.Lines[i]));
            }
        }

    }
}
