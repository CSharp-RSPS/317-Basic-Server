using RSPS.src.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    public class SendSkill : ISendPacket
    {

        private int Level;
        private int Experience;
        private int SkillId;

        public SendSkill(Skill skill)
        {

            SkillId = (int)skill.SkillType;
            Level = skill.CurrentLevel + skill.VisibleBoost;
            Experience = skill.CurrentExperience;
        }

        public byte[] SendPacket(ISAACCipher encryptor)
        {
            PacketWriter packetWriter = Packet.CreatePacketWriter(7);
            packetWriter.WriteHeader(encryptor, 134);
            packetWriter.WriteByte(SkillId);
            packetWriter.WriteInt(Experience, Packet.ByteOrder.MIDDLE);
            packetWriter.WriteByte(Level);
            return packetWriter.Payload;
        }
    }
}
