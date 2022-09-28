﻿using RSPS.src.entity.player;
using RSPS.src.entity.update.block.pblock;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.update.block
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

        public void Process(Player player, PacketWriter writer)
        {
            if (!player.Flags.IsUpdateNeeded() && !ForceAppearance)
            {
                return;
            }

            // First we must calculate and write the mask.
            int mask = 0x0;

            if (player.Flags.IsFlagged(flag.FlagType.PublicChat) && !NoPublicChat)
            {
                mask |= 0x80;
            }

            if (player.Flags.IsFlagged(flag.FlagType.Appearance) || ForceAppearance)
            {
                mask |= 0x10;
            }


            //now we write the mask
            if (mask >= 0x100)
            {
                mask |= 0x40;
                writer.WriteShortLittleEndian(mask);
            }
            else
            {
                writer.WriteByte(mask);
            }

            //Graphics
            //Animation
            //FOrced Chat
            if (player.Flags.IsFlagged(flag.FlagType.PublicChat) && !NoPublicChat)
            {
                new PlayerPublicChatBlock().ProcessBlock(player, writer);
            }

            //Face Entity
            //Appearance
            if (player.Flags.IsFlagged(flag.FlagType.Appearance) || ForceAppearance)
            {
                new PlayerAppearanceBlock().ProcessBlock(player, writer);
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