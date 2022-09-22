using RSPS.src.entity.movement;
using RSPS.src.entity.player.skill;
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

namespace RSPS.src.entity.player
{
    /// <summary>
    /// Manages players in a game world
    /// </summary>
    public sealed class PlayerManager : EntityManager<Player>
    {

        private static readonly int[] SidebarInterfaceIds = {
            //3917, 638, 3213, 1644, 5608, 1151, 5065, 5715, 2449, 904, 147, 6299, 2423
            2423, 3917, 638, 3213, 1644, 5608, 1151, 18128, 5065, 5715, 2449, 904, 147, 962
        };

        /// <summary>
        /// Holds the players pending login
        /// </summary>
        public Queue<Player> PendingLogin = new();

        /// <summary>
        /// Holds the players that disconnected without logging out properly
        /// </summary>
        public List<Player> Disconnected = new();

        /// <summary>
        /// Holds the players pending removal from the world
        /// </summary>
        public Queue<Player> PendingRemoval = new();


        public override int GetIndex(Player entity)
        {
            return base.GetIndex(entity) + 1;
        }

        public override Player Add(Player entity) {
            base.Add(entity);

            entity.PlayerIndex = GetIndex(entity);

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
            // Send the side bars
            for (int i = 0; i < SidebarInterfaceIds.Length; i++)
            {
                PacketHandler.SendPacket(player, new SendSidebarInterface(i, SidebarInterfaceIds[i]));
            }
            // Send the skills in the skill tab
            foreach (SkillType skill in Enum.GetValues(typeof(SkillType)))
            {
                player.Skills.Add(skill != SkillType.HITPOINTS ? new Skill(skill) : new Skill(skill, 10, 1300));
                PacketHandler.SendPacket(player, new SendSkill(player.GetSkill(skill)));
            }
            // Send the initial map region
            PacketHandler.SendPacket(player, new SendMapRegion(player));
            // Send the run energy
            PacketHandler.SendPacket(player, new SendRunEnergy(player.MovementHandler.Energy));

            //player.NeedsPlacement = true; - already sent from MapRegion packet
            //player.Flags.UpdateFlag(flag.FlagType.APPEARANCE, true);
            player.AppearanceUpdateRequired = true;
            player.RequestUpdate();
        }

        /// <summary> 
        /// Logs a player on, into the game world
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="worldDetails">Details about the world the player is logging on to</param>
        public static void Login(Player player, WorldDetails worldDetails)
        {
            if (worldDetails.Debugging)
            {
                PacketHandler.SendPacket(player.PlayerConnection, new SendMessage(string.Format("You are at {0}", player.Position.ToString())));
                PacketHandler.SendPacket(player.PlayerConnection, new SendMessage(
                    string.Format("Index: {0}; Flagged: {1}; Rights: {2}", player.PlayerIndex, player.Flagged, player.Rights)));
            }
            PacketHandler.SendPacket(player.PlayerConnection, new SendMessage(string.Format("Welcome to {0}.", worldDetails.Name)));
            PacketHandler.SendPacket(player.PlayerConnection, new SendMessage(string.Format("Global notice: {0}.", "blablabla")));
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
