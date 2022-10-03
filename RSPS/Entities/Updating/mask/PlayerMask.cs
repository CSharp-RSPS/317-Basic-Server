using RSPS.Entities.Mobiles.Players;
using RSPS.Entities.Updating.block.pblock;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating.block
{
    public class PlayerMask : IUpdateProtocol<Player>
    {

        private bool ForceAppearance;
        private bool NoPublicChat;

        public PlayerMask(bool forceAppearance, bool noPublicChat)
        {
            ForceAppearance = forceAppearance;
            NoPublicChat = noPublicChat;
        }

        public void Process(Player player, PacketWriter stateBlock, PacketWriter writer)
        {
            if (!player.UpdateRequired && !ForceAppearance)
            {
                return;
            }

            // First we must calculate and write the mask.
            int mask = 0x0;

            if (player.ChatUpdateRequired && !NoPublicChat)
            {
                mask |= 0x80;
            }

            if (player.AppearanceUpdateRequired || ForceAppearance)
            {
                mask |= 0x10;
            }


            //now we write the mask
            if (mask >= 0x100)
            {
                mask |= 0x40;
                stateBlock.WriteShortLittleEndian(mask);
            }
            else
            {
                stateBlock.WriteByte(mask);
            }

            //Graphics
            //Animation
            //FOrced Chat
            if (player.ChatUpdateRequired && !NoPublicChat)
            {
                new PlayerPublicChatBlock().ProcessBlock(player, stateBlock);
            }

            //Face Entity
            //Appearance
            if (player.AppearanceUpdateRequired || ForceAppearance)
            {
                new PlayerAppearanceBlock().ProcessBlock(player, stateBlock);
            }
            //Face Coords
            //Primary Hit
            //Secondary Hit

            // Cache and write the result
            //block.WriteBytes(result.memoryStream.GetBuffer());
            //ctx.setBuffer(forceAppearance, noPublicChat, result.getBuffer()); - no buffer writing at this time
        }
    }
}
