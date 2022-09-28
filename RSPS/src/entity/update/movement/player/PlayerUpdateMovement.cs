using RSPS.src.entity.player;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update.movement.player
{
    public class PlayerUpdateMovement
    {

        private Player player;
        private PacketWriter writer;

        public PlayerUpdateMovement(Player player, PacketWriter writer)
        {
            this.player = player;
            this.writer = writer;
        }

        public void UpdateMyMovement()
        {

        }

        public void UpdateOtherMovement()
        {

        }

    }
}
