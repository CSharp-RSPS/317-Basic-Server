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
    /// Sends a skill level to the client.
    /// </summary>
    [PacketDef(PacketDefinition.SkillLevel)]
    public sealed class SendSkill : IPacketPayloadBuilder
    {

        public int Level { get; private set; }
        public int Experience { get; private set; }
        public int SkillId { get; private set; }

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
