using RSPS.src.entity.movement.Locations;
using RSPS.src.entity.player;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update
{
    internal class PlayerUpdate : EntityUpdate<Player>, IUpdateProtocol<Player>
    {
        public PlayerUpdate(Player t, PacketWriter writer) : base(t, writer)
        {
        }

        public void Process(Player player, PacketWriter writer)
        {
            throw new NotImplementedException();
        }

        private void AddNewPlayer(PacketWriter writer, Player otherPlayer)
        {
            writer.WriteBits(11, otherPlayer.WorldIndex);//Server slot
            writer.WriteBit(true);// Yes an update is required
            writer.WriteBit(true);// Discard walking queue

            //Position delta = Position.Delta(player.Position, )
        }
    }
}
