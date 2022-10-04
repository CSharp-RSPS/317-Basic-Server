using RSPS.Entities.Health;
using RSPS.Entities.Mobiles;
using RSPS.Entities.Mobiles.Npcs;
using RSPS.Entities.Mobiles.Players.Events;
using RSPS.Entities.Mobiles.Players.Variables;
using RSPS.Entities.movement;
using RSPS.Entities.movement.Locations;
using RSPS.Game.Comms.Chatting;
using RSPS.Game.Comms.Messaging;
using RSPS.Game.Items.Containers;
using RSPS.Game.Skills;
using RSPS.Game.UI;
using RSPS.Net.Codec;
using RSPS.Net.Connections;
using RSPS.Net.GamePackets;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Schedule;
using RSPS.Schedule.Impl;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace RSPS.Entities.Mobiles.Players
{
    /// <summary>
    /// Represents a human playable character
    /// </summary>
    public class Player : Mobile, ICombatable
    {

        /// <summary>
        /// The player's client connection
        /// </summary>
        public Connection PlayerConnection { get; private set; }

        /// <summary>
        /// Holds the player credentials
        /// </summary>
        public PlayerCredentials Credentials { get; private set; }

        /// <summary>
        /// Retrieves the player movement
        /// </summary>
        public PlayerMovement PlayerMovement => (PlayerMovement)Movement;

        /// <summary>
        /// The player's persistent global variables
        /// </summary>
        public PersistentVariables PersistentVars { get; private set; }

        /// <summary>
        /// The player's non-persistent global variables
        /// </summary>
        public NonPersistentVariables NonPersistentVars { get; private set; }

        /// <summary>
        /// The login state of the player
        /// </summary>
        public bool LoggedIn { get; set; }

        /// <summary>
        /// The inventory item container
        /// </summary>
        public ItemContainer Inventory { get; private set; }

        /// <summary>
        /// The bank item container
        /// </summary>
        public ItemContainer Bank { get; private set; }

        /// <summary>
        /// The equipment item container
        /// </summary>
        public ItemContainer Equipment { get; private set; }

        /// <summary>
        /// Handles communication for the player
        /// </summary>
        public Communication Comms { get; private set; }

        /// <summary>
        /// Controls player events
        /// </summary>
        public PlayerEventController PlayerEvents { get; private set; }

        public Stopwatch IdleTimer = new Stopwatch();
       

        public List<Skill> Skills = new List<Skill>();

        public Appearance Appearance = new Appearance();

        public dynamic Combat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public dynamic FightType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoRetialate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Hitpoints Hitpoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        
        //public bool UpdateRequired = false;
        public bool AppearanceUpdateRequired = false;
        public bool ChatUpdateRequired = false;

        public Player(PlayerCredentials credentials, Connection playerConnection)
            : base(new Position(3222 + new Random().Next(-1, 4), 3222 + new Random().Next(0, 6)), 
                  new PlayerMovement())
        {
            Credentials = credentials;
            PersistentVars = new PersistentVariables();
            NonPersistentVars = new NonPersistentVariables();

            PlayerConnection = playerConnection;

            Inventory = new ItemContainer(ItemContainerType.Inventory, PersistentVars.Member);
            Bank = new ItemContainer(ItemContainerType.Bank, PersistentVars.Member);
            Equipment = new ItemContainer(ItemContainerType.Equipment, PersistentVars.Member);

            Comms = new Communication();
            PlayerEvents = new PlayerEventController();

            //Scheduler.AddJob(new PlayerWalkingJob("Player Walking Job", this, TimeSpan.FromMilliseconds(600)));
        }

        public Player RequestUpdate()
        {
            UpdateRequired = true;
            return this;
        }

        public override void ResetFlags()
        {
            //Flags.ResetUpdateFlags();
            UpdateRequired = false;
            AppearanceUpdateRequired = false;
            ChatUpdateRequired = false;
            ResetMovementQueue = false;
            NeedsPlacement = false;
        }

        public Skill GetSkill(SkillType skillType)
        {
            return Skills.First(s => s.SkillType == skillType);
        }

    }
}
