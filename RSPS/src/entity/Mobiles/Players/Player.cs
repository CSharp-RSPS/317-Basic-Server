using RSPS.src.entity.Health;
using RSPS.src.entity.Mobiles;
using RSPS.src.entity.Mobiles.Npcs;
using RSPS.src.entity.Mobiles.Players.Skills;
using RSPS.src.entity.movement;
using RSPS.src.entity.movement.Locations;
using RSPS.src.game.comms.chat;
using RSPS.src.game.Items.Containers;
using RSPS.src.net.Codec;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using RSPS.src.schedule;
using RSPS.src.schedule.impl;
using System.Diagnostics;

namespace RSPS.src.entity.Mobiles.Players
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
        /// The administrative rights of the player
        /// </summary>
        public PlayerRights Rights { get; set; }

        /// <summary>
        /// Whether the player is flagged
        /// </summary>
        public bool Flagged { get; set; }

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



        public Stopwatch IdleTimer = new Stopwatch();




        public List<long> Ignores { get; private set; } = new();
        public List<long> Friends { get; private set; } = new();
       

        public List<Skill> Skills = new List<Skill>();

        public Appearance Appearance = new Appearance();

        public dynamic Combat { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public dynamic FightType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool AutoRetialate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Hitpoints Hitpoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        
        public bool UpdateRequired = false;
        public bool AppearanceUpdateRequired = false;
        public bool ChatUpdateRequired = false;

        public ChatMessage ChatMessage;

        public Player(PlayerCredentials credentials, Connection playerConnection)
            : base(new Position(3222 + new Random().Next(-1, 4), 3222 + new Random().Next(0, 6)), 
                  new PlayerMovement())
        {
            Credentials = credentials;
            PlayerConnection = playerConnection;
            Inventory = new ItemContainer(28, true, false);
            Bank = new ItemContainer(256, true, true);
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
