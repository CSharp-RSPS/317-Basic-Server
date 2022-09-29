using RSPS.src.entity.Mobiles;
using RSPS.src.entity.movement;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RSPS.src.entity.player
{
    /// <summary>
    /// Handles the movement of a player
    /// </summary>
    public sealed class PlayerMovement : MobileMovement
    {

        /// <summary>
        /// Whether the player is running
        /// </summary>
        public bool Running { get; set; }

        /// <summary>
        /// The run energy
        /// </summary>
        public int Energy { get; set; } = MovementHandler.MaxRunEnergy;

        /// <summary>
        /// The weight of what the player is carrying with them
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Whether the running queue is enabled
        /// </summary>
        public bool RunningQueueEnabled { get; set; }

        /// <summary>
        /// Whether the map region has changed
        /// </summary>
        public bool MapRegionChanged { get; set; } = true;


        /// <summary>
        /// Resets the player movement to prepare for the next movement
        /// </summary>
        /// <returns>The instance</returns>
        public static void Reset(Player player)
        {
            player.Movement.FollowLeader = null;
            player.ResetWalkTo();

            //TODO: if player has an active action, interrupt
            //TODO: Mark as inactive if in combat

            //TODO: Add entity focus state update(null)
        }

        /// <summary>
        /// Updates the position of a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="writer">The packet writer</param>
        public static void UpdatePlayerPosition(Player player, PacketWriter writer)
        {
            if (player.LastPosition == null)
            {
                throw new NullReferenceException(nameof(player.LastPosition));
            }
            writer.WriteBits(1, 1);
            writer.WriteBits(2, 3);
            writer.WriteBits(2, player.Position.Z);
            writer.WriteBits(1, 1);
            writer.WriteBits(1, player.UpdateRequired ? 1 : 0);
            writer.WriteBits(7, player.Position.GetLocalY(player.LastPosition));
            writer.WriteBits(7, player.Position.GetLocalX(player.LastPosition));
        }

        /// <summary>
        /// Updates the personal movement of a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="writer">The packet writer</param>
        public void UpdatePersonal(Player player, PacketWriter writer)
        {
            PlayerMovement playerMovement = (PlayerMovement)player.Movement;

            if (player.Movement.Teleported || playerMovement.MapRegionChanged)
            {
                UpdatePlayerPosition(player, writer);
                return;
            }
            Update(player, writer);
        }

        public override bool CanMove(Mobile mob)
        {
            Player player = (Player)mob;

            //TODO: if banking, shopping, trading, interface opened... => can not move

            return base.CanMove(mob);
        }

        /// <summary>
        /// Processes the run energy depletion for a player
        /// </summary>
        /// <param name="player">The player</param>
        public static void ProcessRunEnergyDepletion(Player player)
        {
            PlayerMovement playerMovement = (PlayerMovement)player.Movement;

            if (playerMovement.Energy <= 0)
            {
                playerMovement.Running = false;
                PacketHandler.SendPacket(player, new SendConfiguration(173, playerMovement.Running));
                return;
            }
            int energyUseDamper = 67 + ((67 * (int)Math.Round(Math.Clamp(playerMovement.Weight, 0, 64))) / 64);
            playerMovement.Energy = Math.Clamp(playerMovement.Energy - energyUseDamper, 0, 10000);

            // TODO: Use agility level
            int agLvl = 100;
            double rpm = 0; // Recovery per minute

            if (agLvl < 10)
            {
                rpm = 8;
            }
            else if (agLvl < 20)
            {
                rpm = 9.2;
            }
            else if (agLvl < 30)
            {
                rpm = 10.7;
            }
            else if (agLvl < 40)
            {
                rpm = 12.2;
            }
            else if (agLvl < 50)
            {
                rpm = 13.8;
            }
            else if (agLvl < 60)
            {
                rpm = 15;
            }
            else if (agLvl < 70)
            {
                rpm = 17;
            }
            else if (agLvl < 80)
            {
                rpm = 19;
            }
            else if (agLvl < 90)
            {
                rpm = 20.8;
            }
            else if (agLvl < 99)
            {
                rpm = 22.7;
            }
            else
            {
                rpm = 24.5;
            }
            double rps = rpm / 10; // Recovery per second

            PacketHandler.SendPacket(player, new SendRunEnergy(playerMovement.Energy));
        }

        /// <summary>
        /// Processes the run energy recovery for a player
        /// </summary>
        /// <param name="player">The player</param>
        public static void ProcessRunEnergyRecovery(Player player)
        {
            PlayerMovement playerMovement = (PlayerMovement)player.Movement;

            if (playerMovement.Energy == 10000)
            {
                return;
            }
            int energyRecovery = (0 / 6) + 8;
            playerMovement.Energy = Math.Clamp(playerMovement.Energy + energyRecovery, 0, 10000);

            PacketHandler.SendPacket(player, new SendRunEnergy(playerMovement.Energy));
        }

    }
}
