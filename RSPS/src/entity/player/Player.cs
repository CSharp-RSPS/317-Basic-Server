using RSPS.src.entity.movement;
using RSPS.src.entity.npc;
using RSPS.src.entity.player.skill;
using RSPS.src.net;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using System.Diagnostics;

namespace RSPS.src.entity.player
{
    public class Player : Entity
    {

        public string Username { get; private set; }
        public string Password { get; private set; }
        public Connection PlayerConnection { get; private set; }

        public Stopwatch IdleTimer = new Stopwatch();

        public Position Position = new Position(3222 + new Random().Next(-1, 4), 3222 + new Random().Next(0, 6));
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

        private static readonly int[] SIDEBAR_INTERFACE_IDS = {
            3917, 638, 3213, 1644, 5608, 1151, 5065, 5715, 2449, 904, 147, 6299, 2423
        };

        public Player(string username, string password, Connection playerConnection)
        {
            this.Username = username;
            this.Password = password;
            this.PlayerConnection = playerConnection;
            MovementHandler = new MovementHandler(this);
        }

        public void LoginPlayer()
        {
            //Console.WriteLine("Player login is now being started");
            //update creditinals, make sure password is good.
            //NO BANS, and maybe login limit?

            //Username = Misc.EncodeBase37(Username); - converts name to long
            PlayerConnection.Player = this;
            MemoryStream loginResponse = new MemoryStream(3);
            loginResponse.WriteByte(2);//response all is good
            loginResponse.WriteByte(1);//player rights
            loginResponse.WriteByte(0);//not clue. comment out when working with new client
            Program.SendGlobalByes(PlayerConnection, loginResponse.GetBuffer());
            
            Program.connections.Enqueue(this);
            //InitializePlayerSession();
        }

        public void InitializePlayerSession()
        {
            World.players.Add(this);
            PlayerIndex = World.players.IndexOf(this);
            //Console.WriteLine("sent interfaces");

            for (int i = 1; i < SIDEBAR_INTERFACE_IDS.Length; i++)
            {
                PacketHandler.SendPacket(PlayerConnection, new SendSidebarInterface(i, SIDEBAR_INTERFACE_IDS[i]));
            }

            foreach (SkillType skill in Enum.GetValues(typeof(SkillType)))
            {
                if (skill != SkillType.HITPOINTS)
                {
                    Skills.Add(new Skill(skill));
                    PacketHandler.SendPacket(PlayerConnection, new SendSkill(GetSkill(skill)));
                    continue;
                }
                Skills.Add(new Skill(skill, 10, 1300));
                PacketHandler.SendPacket(PlayerConnection, new SendSkill(GetSkill(skill)));
            }



            PacketHandler.SendPacket(PlayerConnection, new SendMapRegion(this));
            PacketHandler.SendPacket(PlayerConnection, new SendMessage("a"));//was welcome to Constants.SERVER_NAME!
            PacketHandler.SendPacket(PlayerConnection, new SendRunEnergy(MovementHandler.Energy));
            
            NeedsPlacement = true;
            AppearanceUpdateRequired = true;
            UpdateRequired = true;
            
            //PlayerUpdating.Update(this);
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

        //private int CalculateRunEnergyDepletion(int energy)
        //{
        //    Console.WriteLine("Depleting Energy");
        //    if (energy == 0)
        //    {
        //        Console.WriteLine("Energy should be 0. Current energy level: {0}", energy);
        //        return 0;
        //    }
        //    int EnergyUseDamper = 67 + ((67 * Math.Clamp(MovementHandler.Weight, 0, 64)) / 64);
        //    return Math.Clamp(energy / EnergyUseDamper, 0, 10000);
        //}

        //private int CalculateRunEnergyRecovery(int energy)
        //{
        //    Console.WriteLine("Restoring Energy");
        //    //get the agility skill
        //    if (energy == 10000)
        //    {
        //        Console.WriteLine("Energy should be 10000. Current energy level: {0}", energy);
        //        return energy;
        //    }
        //    int EnergyRecovery = (0 / 6) + 8;
        //    return Math.Clamp(energy + EnergyRecovery, 0, 10000);
        //}

    }
}
