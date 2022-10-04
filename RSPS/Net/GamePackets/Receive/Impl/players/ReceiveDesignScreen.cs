using RSPS.Entities.Mobiles.Players;
using RSPS.Net.GamePackets.Send;
using RSPS.Net.GamePackets.Send.Impl;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace RSPS.Net.GamePackets.Receive.Impl
{

    /// <summary>
    /// This packet is sent when a player is choosing their character design options.
    /// </summary>
    [PacketInfo(101, 13)]
    public sealed class ReceiveDesignScreen : IReceivePacket
    {


        public void ReceivePacket(Player player, PacketReader reader)
        {
            player.Appearance.Gender = reader.ReadByte();
            player.Appearance.Head = reader.ReadByte();
            player.Appearance.Beard = reader.ReadByte();
            player.Appearance.Chest = reader.ReadByte();
            player.Appearance.Arms = reader.ReadByte();
            player.Appearance.Hands = reader.ReadByte();
            player.Appearance.Legs = reader.ReadByte();
            player.Appearance.Feet = reader.ReadByte();
            player.Appearance.HairColor = reader.ReadByte();
            player.Appearance.TorsoColor = reader.ReadByte();
            player.Appearance.LegColor = reader.ReadByte();
            player.Appearance.FeetColor = reader.ReadByte();
            player.Appearance.SkinColor = reader.ReadByte();

            //player.Flags.UpdateFlag(FlagType.Appearance, true);

            player.AppearanceUpdateRequired = true;
            player.UpdateRequired = true;

            PacketHandler.SendPacket(player, PacketDefinition.ClearScreen);
        }

    }
}
