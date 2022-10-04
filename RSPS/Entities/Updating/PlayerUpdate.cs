using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.Updating.block;
using RSPS.Entities.Updating.Local;
using RSPS.Net.GamePackets;

namespace RSPS.Entities.Updating
{
    public class PlayerUpdate : EntityUpdate<Player>
    {
        public PlayerUpdate(Player player, PacketWriter payload, PacketWriter maskBlock) : base(player, payload, maskBlock)
        {}

        public void Process()
        {
            if (Mobile == null)
            {
                return;
            }

            Mobile.PlayerMovement.UpdatePersonal(Mobile, Packet);

            if (Mobile.UpdateRequired)
            {
                new PlayerMask().NoPublicChat().Process(Mobile, UpdateBlock);
            }

            new ProcessLocalPlayer().Process(Mobile, Packet, UpdateBlock);
            new UpdateLocalPlayer().Process(Mobile, Packet, UpdateBlock);
        }
    }
}
