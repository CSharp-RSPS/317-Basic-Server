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
    public class PlayerManager : EntityManager<Player>
    {

        private static readonly int[] SIDEBAR_INTERFACE_IDS = {
            3917, 638, 3213, 1644, 5608, 1151, 5065, 5715, 2449, 904, 147, 6299, 2423
        };


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
        /// Initializes a new player session
        /// </summary>
        /// <param name="player">The player</param>
        public void InitializeSession(Player player)
        {
            //NO BANS, and maybe login limit?

            //Console.WriteLine("sent interfaces");

            for (int i = 1; i < SIDEBAR_INTERFACE_IDS.Length; i++)
            {
                PacketHandler.SendPacket(player, new SendSidebarInterface(i, SIDEBAR_INTERFACE_IDS[i]));
            }

            foreach (SkillType skill in Enum.GetValues(typeof(SkillType)))
            {
                if (skill != SkillType.HITPOINTS)
                {
                    player.Skills.Add(new Skill(skill));
                    PacketHandler.SendPacket(player, new SendSkill(player.GetSkill(skill)));
                    continue;
                }
                player.Skills.Add(new Skill(skill, 10, 1300));
                PacketHandler.SendPacket(player, new SendSkill(player.GetSkill(skill)));
            }
            PacketHandler.SendPacket(player, new SendMapRegion(player));
            PacketHandler.SendPacket(player, new SendRunEnergy(player.MovementHandler.Energy));

            player.NeedsPlacement = true;
            player.AppearanceUpdateRequired = true;
            player.UpdateRequired = true;
        }

        /// <summary>
        /// Logs a player on, into the game world
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="worldDetails">Details about the world the player is logging on to</param>
        public void Login(Player player, WorldDetails worldDetails)
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
        }

        /// <summary>
        /// Logs the player out
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="disconnected">Whether the player is disconnected already from their end</param>
        public bool Logout(Player player, bool disconnected = false) {
            //TODO logout logic
            //don't allow logout when in a fight etc
            return disconnected || true;
        }

        public override void Dispose() {
            GC.SuppressFinalize(this);

            // Logout all players
            Entities.ForEach(p => Logout(p));

            base.Dispose();
        }

    }
}
