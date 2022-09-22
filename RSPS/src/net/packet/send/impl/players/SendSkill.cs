using RSPS.src.entity.player;
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
    public sealed class SendSkill : ISendPacket
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

        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            PacketWriter packetWriter = Packet.CreatePacketWriter(7);
            packetWriter.WriteHeader(encryptor, 134);
            packetWriter.WriteByte(SkillId);
            packetWriter.WriteInt(Experience, Packet.ByteOrder.MiddleEndian);
            packetWriter.WriteByte(Level);
            return packetWriter;
        }
    }
}
