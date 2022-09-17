using RSPS.src.entity.movement;
using RSPS.src.entity.npc;
using RSPS.src.entity.player.skill;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using System.Diagnostics;

namespace RSPS.src.entity.player
{
    /// <summary>
    /// Represents a human playable character
    /// </summary>
    public class Player : Entity
    {

        /// <summary>
        /// Holds the player credentials
        /// </summary>
        public PlayerCredentials Credentials { get; private set; }

        /// <summary>
        /// The administrative rights of the player
        /// </summary>
        public PlayerRights Rights { get; set; }

        /// <summary>
        /// Whether the player is flagged
        /// </summary>
        public bool Flagged { get; set; }

        /// <summary>
        /// The player's client connection
        /// </summary>
        public Connection PlayerConnection { get; private set; }

        public Stopwatch IdleTimer = new Stopwatch();

        /// <summary>
        /// The current position
        /// </summary>
        public Position Position { get; private set; }

        //public Position Position = new Position(3222, 3222);
        public Position CurrentRegion = new Position(0, 0);

        public List<Skill> Skills = new List<Skill>();

        public List<Player> LocalPlayers = new List<Player>();
        public List<Npc> LocalNpcs = new List<Npc>();
        public Appearance Appearance = new Appearance();

        public MovementHandler MovementHandler { get; private set; }

        public int PrimaryDirection = -1;
        public int SecondaryDirection = -1;

        public bool NeedsPlacement { get; set; }
        public bool UpdateRequired { get; set; }
        public bool ResetMovementQueue { get; set; }
        public bool ChatUpdateRequired { get; set; }
        public bool AppearanceUpdateRequired { get; set; }

        public int ChatColor;
        public int ChatEffects;
        public byte[] ChatText;

        public int PlayerIndex;

        public Player(PlayerCredentials credentials, Connection playerConnection)
        {
            Credentials = credentials;
            Position = new Position(3222 + new Random().Next(-1, 4), 3222 + new Random().Next(0, 6));
            PlayerConnection = playerConnection;
            MovementHandler = new MovementHandler(this);
        }

        public Player RequestUpdate()
        {
            UpdateRequired = true;
            return this;
        }

        public override void ResetFlags()
        {
            UpdateRequired = false;
            AppearanceUpdateRequired= false;
            ChatUpdateRequired= false;
            ResetMovementQueue = false;
            NeedsPlacement = false;
            PrimaryDirection = -1;
            SecondaryDirection = -1;
        }

        public Skill GetSkill(SkillType skillType)
        {
            return Skills[(int)skillType];
        }
    }
}
