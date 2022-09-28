using RSPS.src.entity.movement.Locations;
using RSPS.src.entity.player;
using RSPS.src.entity.update.block;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update
{
    internal class PlayerUpdate : EntityUpdate<Player>
    {

        public PlayerUpdate(Player player, PacketWriter writer) : base(player, writer)
        {}

        public void Process()
        {
            if (Entity.Flags.IsUpdateNeeded())
            {
                new PlayerMask(false, true).Process(Entity, writer);
            }
            base.ExecuteUpdates();
        }
    }
}
