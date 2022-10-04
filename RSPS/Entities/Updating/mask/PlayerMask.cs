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
    public class PlayerMask
    {

        private bool forceAppearance = false;
        private bool noPublicChat = false;

        public PlayerMask()
        {}

        public void Process(Player player, PacketWriter updateBlock)
        {
/*            if (!player.UpdateRequired && !forceAppearance)
            {
                return;
            }*/

            // First we must calculate and write the mask.
            int mask = 0x0;

            if (player.ChatUpdateRequired && !noPublicChat)
            {
                mask |= 0x80;
            }

            if (player.AppearanceUpdateRequired || forceAppearance)
            {
                mask |= 0x10;
            }


            //now we write the mask
            if (mask >= 0x100)
            {
                mask |= 0x40;
                updateBlock.WriteShortLittleEndian(mask);
            }
            else
            {
                updateBlock.WriteByte(mask);
            }

            //Graphics
            //Animation
            //FOrced Chat
            if (player.ChatUpdateRequired && !noPublicChat)
            {
                new PlayerPublicChatBlock().AppendBlock(player, updateBlock);
            }

            //Face Entity
            //Appearance
            if (player.AppearanceUpdateRequired || forceAppearance)
            {
                new PlayerAppearanceBlock().AppendBlock(player, updateBlock);
            }
            //Face Coords
            //Primary Hit
            //Secondary Hit

            // Cache and write the result
            //block.WriteBytes(result.memoryStream.GetBuffer());
            //ctx.setBuffer(forceAppearance, noPublicChat, result.getBuffer()); - no buffer writing at this time
        }

        public PlayerMask ForceAppearance()
        {
            forceAppearance = true;
            return this;
        }

        public PlayerMask NoPublicChat()
        {
            noPublicChat = true;
            return this;
        }

    }
}
