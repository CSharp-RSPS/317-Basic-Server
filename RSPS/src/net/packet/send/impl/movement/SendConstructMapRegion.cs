using RSPS.src.Util.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    /// <summary>
    /// Constructs a dynamic map region using a palette of 8*8 tiles.
    /// </summary>
    [PacketDef(PacketDefinition.ConstructMapRegion)]
    public sealed class SendConstructMapRegion : IPacketVariablePayloadBuilder
    {

        /// <summary>
        /// The region X coordinate
        /// </summary>
        public int RegionX { get; private set; }

        /// <summary>
        /// The region Y coordinate
        /// </summary>
        public int RegionY { get; private set; }

        
        /// <summary>
        /// Creates a new construct map region payload builder
        /// </summary>
        /// <param name="regionX">The region X coordinate</param>
        /// <param name="regionY">The region Y coordinate</param>
        public SendConstructMapRegion(int regionX, int regionY)
        {
            RegionX = regionX;
            RegionY = regionY;
        }

        public int GetPayloadSize()
        {
            //TODO
            // Determine how many regions we max. load, multiply by 27 bits, divide by 8 for the bytes, then add 4 bytes for the coordinate shorts
            return 5096;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShort(RegionY, Packet.ValueType.Additional);
            // Write the bit block containing the "palette" of map regions to make up the new region
            writer.SetAccessType(Packet.AccessType.BitAccess);

            for (int z = 0; z < 4; z++)
            {
                for (int x = 0; x < 13; x++)
                {
                    for (int y = 0; y < 13; y++)
                    {
                        //1 bit - set to 0 to indicate to display nothing, 1 to display a region
                        //26 bits - if the flag above is set to 1: region_x << 14 | region_y << 3
                    }
                }
            }
            writer.SetAccessType(Packet.AccessType.ByteAccess); // Is this required?
            writer.WriteShort(RegionX);
        }

    }

}
