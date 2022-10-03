using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Skills;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Updating.block.pblock
{
    public class PlayerAppearanceBlock : IUpdateMask<Player>
    {

        private static readonly int MAX_APPEARANCE_BUFFER_SIZE = 58;

        public void ProcessBlock(Player player, PacketWriter writer)
        {
            try
            {
                PacketWriter block = new(MAX_APPEARANCE_BUFFER_SIZE);

                block.WriteByte(player.Appearance.Gender);
                block.WriteByte(-1);//prayer icon : -1 off 6 is max
                block.WriteByte(-1); // Skull icon : -1 off 1 is max

                block.WriteByte(0);//hat
                block.WriteByte(0);//cape
                block.WriteByte(0);//amulet
                block.WriteByte(0);//weapon
                block.WriteShort(0x100 + player.Appearance.Chest);//chest
                block.WriteByte(0);//sheild

                block.WriteShort(0x100 + player.Appearance.Arms);//arms
                block.WriteShort(0x100 + player.Appearance.Legs);//legs
                block.WriteShort(0x100 + player.Appearance.Head);//head
                block.WriteShort(0x100 + player.Appearance.Hands);//hands
                block.WriteShort(0x100 + player.Appearance.Feet);//feet
                block.WriteShort(0x100 + player.Appearance.Beard);//beard

                block.WriteByte(player.Appearance.HairColor);//hair
                block.WriteByte(player.Appearance.TorsoColor);//torso
                block.WriteByte(player.Appearance.LegColor);//leg
                block.WriteByte(player.Appearance.FeetColor);//feet
                block.WriteByte(player.Appearance.SkinColor);//skin

                // Movement animations
                block.WriteShort(0x328); // stand
                block.WriteShort(0x337); // stand turn
                block.WriteShort(0x333); // walk
                block.WriteShort(0x334); // turn 180
                block.WriteShort(0x335); // turn 90 cw
                block.WriteShort(0x336); // turn 90 ccw
                block.WriteShort(0x338); // run

                block.WriteLong(player.Credentials.UsernameAsLong);
                block.WriteByte(SkillHandler.CalculateCombatLevel(player));//combat level
                block.WriteShort(SkillHandler.GetTotalLevel(player));//total level
                                                                     //45 bytes total?

                // Append the block length and the block to the packet.
                writer.WriteByteNegated(block.Pointer);
                writer.WriteBytes(block.Buffer, block.Pointer);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exeception Caught in Player Appearance block. Error below");
                Debug.WriteLine(e.ToString());
                player.LoggedIn = false;
            }
        }
    }
}
