using RSPS.Entities.Mobiles.Players;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Makes the client load the specified map region.
    /// There are various loops/arrays within the map region loading functionality of the client which have been misunderstood by many.
    /// Loop type	Description
    /// 104 x 104	Maximum size of the client's load area
    /// 8 x 8	Load blocks to speed up loading NPCs, Items and Objects
    /// 13 x 13	Number of load blocks to load
    /// </summary>
    [PacketDef(SendPacketDefinition.LoadMapRegion)]
    public sealed class SendLoadMapRegion : IPacketPayloadBuilder
    {

        /// <summary>
        /// The region X coordinate - (absolute X / 8) plus 6
        /// </summary>
        //public int RegionX { get; private set; }

        /// <summary>
        /// The region Y coordinate - (absolute Y / 8) plus 6
        /// </summary>
      //  public int RegionY { get; private set; }

        public Player Player { get; private set; }


        /// <summary>
        /// Creates a new load map region packet payload builder
        /// </summary>
        /// <param name="regionX">The region X coordinate - (absolute X / 8) plus 6</param>
        /// <param name="regionY">The region Y coordinate - (absolute Y / 8) plus 6</param>
        public SendLoadMapRegion(Player player)
        {
            //RegionX = regionX;
            //RegionY = regionY;
            Player = player;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortAdditional(Player.Position.RegionX + 6);
            writer.WriteShort(Player.Position.RegionY + 6);

            Player.LastPosition = Player.Position.Copy();
        }
    }
}
