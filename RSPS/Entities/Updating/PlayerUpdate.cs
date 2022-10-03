using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.Updating.block;
using RSPS.Entities.Updating.Local;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating
{
    internal class PlayerUpdate : EntityUpdate<Player>
    {
        public PlayerUpdate(Player player, PacketWriter payload, PacketWriter maskBlock) : base(player, payload, maskBlock)
        {}

        public void Process()
        {
            Entity.PlayerMovement.UpdatePersonal(Entity, writer);

            if (Entity.UpdateRequired)
            {
                new PlayerMask(false, true).Process(Entity, maskPayload, writer);
            }

            new ProcessLocalPlayer().Process(Entity, writer, maskPayload);
            new UpdateLocalPlayer().Process(Entity, writer, maskPayload);
        }
    }
}
