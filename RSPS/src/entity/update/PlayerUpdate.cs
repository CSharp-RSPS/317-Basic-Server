using RSPS.src.entity.movement.Locations;
using RSPS.src.entity.player;
using RSPS.src.entity.update.block;
using RSPS.src.entity.update.movement.player;
using RSPS.src.entity.update.player.local;
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

        PacketWriter stateBlock = new(1024);

        public PlayerUpdate(Player t, PacketWriter Writer) : base(t, Writer)
        {
        }

        public void Process(Player player, PacketWriter writer, PacketWriter stateBlock)
        {
            new PlayerUpdateMovement(player, writer).UpdateMyMovement();
            if (player.UpdateRequired)
            {
                new PlayerMask(false, false).Process(player, writer, stateBlock);
            }

            new ProcessLocalPlayers().Process(player, writer, stateBlock);
            new UpdateLocalPlayers().Process(player, writer, stateBlock);

        }
    }
}
