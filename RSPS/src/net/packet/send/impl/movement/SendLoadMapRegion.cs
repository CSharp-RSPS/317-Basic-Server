using RSPS.src.entity.player;
using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    [PacketDef(PacketDefinition.LoadMapRegion)]
    public sealed class SendLoadMapRegion : IPacketPayloadBuilder
    {

        private readonly Player Player;

        public SendLoadMapRegion(Player player)
        {
            this.Player = player;
        }

        public void WritePayload(PacketWriter writer)
        {
            Player.CurrentRegion.SetNewPosition(Player.Position);
            Player.NeedsPlacement = true;

            writer.WriteShort(Player.Position.GetRegionX() + 6, Packet.ValueType.Additional);
            writer.WriteShort(Player.Position.GetRegionY() + 6);
        }
    }
}
