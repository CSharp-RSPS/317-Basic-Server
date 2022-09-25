using RSPS.src.entity.player;
using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Makes the client load the specified map region.
    /// There are various loops/arrays within the map region loading functionality of the client which have been misunderstood by many.
    /// Loop type	Description
    /// 104 x 104	Maximum size of the client's load area
    /// 8 x 8	Load blocks to speed up loading NPCs, Items and Objects
    /// 13 x 13	Number of load blocks to load
    /// </summary>
    [PacketDef(PacketDefinition.LoadMapRegion)]
    public sealed class SendLoadMapRegion : IPacketPayloadBuilder
    {

        /// <summary>
        /// The region X coordinate - (absolute X / 8) plus 6
        /// </summary>
        public int RegionX { get; private set; }

        /// <summary>
        /// The region Y coordinate - (absolute Y / 8) plus 6
        /// </summary>
        public int RegionY { get; private set; }


        /// <summary>
        /// Creates a new load map region packet payload builder
        /// </summary>
        /// <param name="regionX">The region X coordinate - (absolute X / 8) plus 6</param>
        /// <param name="regionY">The region Y coordinate - (absolute Y / 8) plus 6</param>
        public SendLoadMapRegion(int regionX, int regionY)
        {
            RegionX = regionX;
            RegionY = regionY;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(RegionX + 6, Packet.ValueType.Additional);
            writer.WriteShort(RegionY + 6);
        }
    }
}
