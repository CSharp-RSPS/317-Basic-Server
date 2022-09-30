using RSPS.src.entity.movement;
using RSPS.src.net;
using RSPS.src.net.packet.send.impl;
using RSPS.src.net.packet;
using RSPS.src.Worlds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RSPS.src.net.Connections;
using RSPS.src.net.packet.send;
using RSPS.src.entity.update.flag;
using RSPS.src.entity.Mobiles.Players.Skills;
using System.Diagnostics;

namespace RSPS.src.entity.Mobiles.Players
{
    /// <summary>
    /// Manages players in a game world
    /// </summary>
    public sealed class PlayerManager : MobileManager<Player>
    {

        private static readonly int[] SidebarInterfaceIds = {
            //3917, 638, 3213, 1644, 5608, 1151, 5065, 5715, 2449, 904, 147, 6299, 2423
            2423, 3917, 638, 3213, 1644, 5608, 1151, 18128, 5065, 5715, 2449, 904, 147, 962
        };

        /// <summary>
        /// Holds the players that disconnected without logging out properly
        /// </summary>
        public List<Player> Disconnected = new();

        /// <summary>
        /// Holds the players pending removal from the world
        /// </summary>
        public Queue<Player> PendingRemoval = new();


        public override void PrepareTick(Player player)
        {
            base.PrepareTick(player);
        }

        /// <summary>
        /// Handles a game tick for a player
        /// </summary>
        /// <param name="player">The player</param>
        public void OnTick(Player player)
        {
            if (player.PlayerMovement.MapRegionChanged)
            {
                PacketHandler.SendPacket(player, new SendLoadMapRegion(player));
            }
            PacketHandler.SendPacket(player, new SendBeginPlayerUpdating(player));
            PacketHandler.SendPacket(player, new SendNpcUpdating(player));
        }

        public override void FinishTick(Player player)
        {
            player.PlayerMovement.MapRegionChanged = false;

            base.FinishTick(player);
            
            //TODO: if in combat and pending logout, when out of combat => logout
        }

        public override Player Add(Player entity)
        {
            base.Add(entity);

            entity.WorldIndex = GetIndex(entity) + 1;

            return entity;
        }

        public override void Remove(Player entity) {
            //TODO any possible logic we might still need to do
            base.Remove(entity);
        }

        /// <summary>
        /// Attempts to receive a player by their username
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>The result</returns>
        public Player? ByUsername(string username)
        {
            return Entities.FirstOrDefault(p => p.Credentials.Username.ToLower().Equals(username.ToLower()));
        }

        public override void Dispose() {
            GC.SuppressFinalize(this);

            // Logout all players
            Entities.ForEach(p => Logout(p));

            base.Dispose();
        }

        /// <summary>
        /// Initializes a new player session
        /// </summary>
        /// <param name="player">The player</param>
        public static void InitializeSession(Player player)
        {
            PacketHandler.SendPacket(player, new SendInitializePlayer(true, player.WorldIndex));
            PacketHandler.SendPacket(player, PacketDefinition.ResetCamera);
            PacketHandler.SendPacket(player, new SendChatSettings(0, 0, 0));

            // Send the side bars
            for (int i = 0; i < SidebarInterfaceIds.Length; i++)
            { // TODO the right spell book
                PacketHandler.SendPacket(player, new SendSidebarInterface(i, SidebarInterfaceIds[i]));
            }
            // Send the skills in the skill tab
            foreach (SkillType skillType in Enum.GetValues(typeof(SkillType)))
            { // TODO skills from player
                Skill skill = skillType != SkillType.HITPOINTS ? new Skill(skillType) : new Skill(skillType, 10, 1300);
                player.Skills.Add(skill);
                PacketHandler.SendPacket(player, new SendSkill(skill));
            }
      /*      PacketHandler.SendPacket(player, new SendPlayerOption(1, "null"));
            PacketHandler.SendPacket(player, new SendPlayerOption(2, "null"));
            PacketHandler.SendPacket(player, new SendPlayerOption(3, "Follow"));
            PacketHandler.SendPacket(player, new SendPlayerOption(4, "Trade with"));*/

            //TODO: Refresh inventory, equipment, configurations
            //TODO: weapon interface update
            //TODO: initialize pm's
            //TODO: refresh quest tab
            

            // Send the run energy
            //PacketHandler.SendPacket(player, new SendRunEnergy(((PlayerMovement)player.Movement).Energy));

            //player.NeedsPlacement = true; - already sent from MapRegion packet
            player.Flags.UpdateFlag(FlagType.Appearance, true);
            player.AppearanceUpdateRequired = true;
            player.RequestUpdate();
        }

        /// <summary> 
        /// Logs a player on, into the game world
        /// </summary>
        /// <param name="player">The player</param>
        public static void Login(Player player)
        {
            if (WorldHandler.World.Details.Debugging)
            {
                PacketHandler.SendPacket(player, new SendMessage(string.Format("You are at {0}", player.Position.ToString())));
                PacketHandler.SendPacket(player, new SendMessage(
                    string.Format("Index: {0}; Flagged: {1}; Rights: {2}", player.WorldIndex, player.Flagged, player.Rights)));
            }
            PacketHandler.SendPacket(player, new SendMessage(string.Format("Welcome to {0}.", WorldHandler.World.Details.Name)));
            PacketHandler.SendPacket(player, new SendMessage(string.Format("Global notice: {0}.", "blablabla")));
            // if muted etc send msg

            player.LoggedIn = true;
        }

        /// <summary>
        /// Whether a player is able to logout
        /// </summary>
        /// <param name="player">The player</param>
        /// <returns>The result</returns>
        public static bool CanLogout(Player player)
        {
            //TODO: Add logic, e.g. still in combat etc
            return true; 
        }

        /// <summary>
        /// Logs the player out
        /// </summary>
        /// <param name="player">The player</param>
        public static bool Logout(Player player)
        {
            if (!CanLogout(player))
            {
                return false;
            }
            player.LoggedIn = false;
            return true;
        }

    }
}
