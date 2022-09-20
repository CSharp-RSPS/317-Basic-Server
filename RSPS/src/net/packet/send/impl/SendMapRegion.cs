using RSPS.src.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    public sealed class SendMapRegion : ISendPacket
    {

        private readonly Player Player;

        public SendMapRegion(Player player)
        {
            this.Player = player;
        }

        public PacketWriter SendPacket(ISAACCipher encryptor)
        {
            Player.CurrentRegion.SetNewPosition(Player.Position);
            Player.NeedsPlacement = true;
            PacketWriter packetWriter = Packet.CreatePacketWriter(5);
            packetWriter.WriteHeader(encryptor, 73);
            packetWriter.WriteShort(Player.Position.GetRegionX() + 6, Packet.ValueType.Additional);
            packetWriter.WriteShort(Player.Position.GetRegionY() + 6);
            return packetWriter;
        }
    }
}
