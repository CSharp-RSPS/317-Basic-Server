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
            bool updateRequired = player.UpdateRequired;

            if (player.NeedsPlacement) // Do they need placement?
            {
                writer.WriteBit(true); // Yes, there is an update.

                int posX = player.Position.GetLocalX(player.CurrentRegion);
                int posY = player.Position.GetLocalY(player.CurrentRegion);
                // Console.WriteLine("Appending placement for: " + player.Username);//doesn't work for second player
                AppendPlacement(writer, posX, posY, player.Position.Z, player.ResetMovementQueue, updateRequired);
            }
            else
            { // No placement update, check for movement.
                int pDir = player.PrimaryDirection;
                int sDir = player.SecondaryDirection;
                UpdateMovement(writer, pDir, sDir, updateRequired);
            }
        }

        public void UpdateOtherMovement()
        {
            bool updateRequired = player.UpdateRequired;
            int pDir = player.PrimaryDirection;//primary direction
            int sDir = player.SecondaryDirection;//secondary direction
            UpdateMovement(writer, pDir, sDir, updateRequired);
        }

        private static void UpdateMovement(PacketWriter outPacket, int pDir, int sDir, bool updateRequired)
        {
            if (pDir != -1)//// If they moved.
            {
                outPacket.WriteBit(true); // Yes, there is an update.

                if (sDir != -1) // If they ran.
                {
                    AppendRun(outPacket, pDir, sDir, updateRequired);
                }
                else
                { // Movement but no running - they walked.
                    AppendWalk(outPacket, pDir, updateRequired);
                }
            }
            else//// No movement.
            {
                if (updateRequired) // Does the state need to be updated?
                {
                    outPacket.WriteBit(true); // Yes, there is an update.
                    AppendStand(outPacket);
                }
                else
                { // No update whatsoever.
                    outPacket.WriteBit(false);
                }
            }
        }
        private static void AppendStand(PacketWriter outPacket)
        {
            outPacket.WriteBits(2, 0); // 0 - no movement.
        }

        private static void AppendPlacement(PacketWriter outPacket, int localX, int localY, int z,
                                     bool discardMovementQueue, bool attributesUpdate)
        {
            outPacket.WriteBits(2, 3); // 3 - placement.
            outPacket.WriteBits(2, z);
            outPacket.WriteBit(discardMovementQueue);
            outPacket.WriteBit(attributesUpdate);
            outPacket.WriteBits(7, localY);
            outPacket.WriteBits(7, localX);
        }

        private static void AppendWalk(PacketWriter outPacket, int direction, bool attributesUpdate)
        {
            outPacket.WriteBits(2, 1); // 1 - walking.

            // Append the actual sector.
            outPacket.WriteBits(3, direction);
            outPacket.WriteBit(attributesUpdate);
        }

        private static void AppendRun(PacketWriter outPacket, int direction, int direction2, bool attributesUpdate)
        {
            outPacket.WriteBits(2, 2); // 2 - running.

            // Append the actual sector.
            outPacket.WriteBits(3, direction);
            outPacket.WriteBits(3, direction2);
            outPacket.WriteBit(attributesUpdate);
        }

    }
}
