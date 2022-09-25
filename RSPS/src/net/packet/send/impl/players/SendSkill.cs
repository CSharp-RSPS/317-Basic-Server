using RSPS.src.entity.player;
using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
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
            writer.WriteInt(Experience, Packet.ByteOrder.MiddleEndian);
            writer.WriteByte(Level);
        }
    }
}
