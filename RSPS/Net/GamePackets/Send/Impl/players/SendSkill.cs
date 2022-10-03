using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Skills;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// This packet sends a specific skill.
    /// </summary>
    [PacketDef(PacketDefinition.SkillLevel)]
    public sealed class SendSkill : IPacketPayloadBuilder
    {

        /// <summary>
        /// The skill level
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// The experience of the skill
        /// </summary>
        public int Experience { get; private set; }

        /// <summary>
        /// The skill ID
        /// </summary>
        public int SkillId { get; private set; }


        /// <summary>
        /// Creates a new skill packet payload builder
        /// </summary>
        /// <param name="skill">The skill</param>
        public SendSkill(Skill skill)
        {

            SkillId = (int)skill.SkillType;
            Level = skill.CurrentLevel + skill.VisibleBoost;
            Experience = skill.CurrentExperience;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteByte(SkillId);
            writer.WriteIntMiddleEndian(Experience);
            writer.WriteByte(Level);
        }
    }
}
