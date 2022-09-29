using RSPS.src.entity.Mobiles.Players;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.Codec
{
    /// <summary>
    /// Represents a packet decoder based on the RS2 protocol
    /// </summary>
    public sealed class PacketDecoder : IProtocolDecoder
    {

        /// <summary>
        /// The player we're decoding for
        /// </summary>
        private readonly Player _player;


        /// <summary>
        /// Creates a new protocol decoder
        /// </summary>
        /// <param name="player">The player we're decoding packets for</param>
        public PacketDecoder(Player player)
        {
            _player = player;
        }

        public bool Decode(Connection connection, PacketReader reader)
        {
            if (connection.NetworkDecryptor == null)
            {
                Console.Error.WriteLine("Unable to decode packet as no decryptor is present");
                return false;
            }
            while (reader.HasReadableBytes && connection.ConnectionState != ConnectionState.Disconnected)
            {
                // Extract the packet opcode and the corresponding default size
                int packetOpcode = reader.ReadByte();
                packetOpcode = packetOpcode - connection.NetworkDecryptor.getNextValue() & 0xFF;
                int packetPayloadSize = PacketSizes[packetOpcode];

                if (packetPayloadSize == -1)
                { // Resolve the packet size of a dynamic packet
                    if (!reader.HasReadableBytes)
                    { // End of data stream reached
                        Debug.WriteLine("Unable to determine variable packet size for packet {0}, no more data present", packetOpcode);
                        break;
                    }
                    packetPayloadSize = reader.ReadByte();
                }
                if (reader.ReadableBytes < packetPayloadSize)
                { // Break out if there's less readable bytes available than we need to complete the packet
                    break;
                }
                // Retrieve the payload of the packet and write it to a new buffer
                byte[] packetPayloadBuffer = new byte[packetPayloadSize];
                Array.Copy(reader.Buffer, reader.Pointer, packetPayloadBuffer, 0, packetPayloadSize);
                // Consume the bytes used by the handled packet from the data reader
                reader.ReadBytes(packetPayloadSize);

                try
                { // Handle the packet
                    PacketHandler.HandlePacket(_player, new(packetOpcode, packetPayloadSize, packetPayloadBuffer));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Holds the predetermined sizes of packets with the index in the array being the packet opcode.
        /// -1 indiciates a packet has a variable size
        /// </summary>
        /// <remarks>TRISTAN YOU PIECE OF SHIT PASTING 377 SIZES - GOD WILL PUNISH</remarks>
        public static readonly int[] PacketSizes = {
            0, 0, 0, 1, -1, 0, 0, 0, 0, 0, // 0
		    0, 0, 0, 0, 8, 0, 6, 2, 2, 0, // 10
		    0, 2, 0, 6, 0, 12, 0, 0, 0, 0, // 20
		    0, 0, 0, 0, 0, 8, 4, 0, 0, 2, // 30
		    2, 6, 0, 6, 0, -1, 0, 0, 0, 0, // 40
		    0, 0, 0, 12, 0, 0, 0, 8, 8, 12, // 50
		    8, 8, 0, 0, 0, 0, 0, 0, 0, 0, // 60
		    8, 0, 2, 2, 8, 6, 0, -1, 0, 6, // 70
		    0, 0, 0, 0, 0, 1, 4, 6, 0, 0, // 80
		    0, 0, 0, 0, 0, 3, 0, 0, -1, 0, // 90
		    0, 13, 0, -1, 0, 0, 0, 0, 0, 0,// 100
		    0, 0, 0, 0, 0, 0, 0, 6, 0, 0, // 110
		    1, 0, 6, 0, 0, 0, -1, 0, 2, 6, // 120
		    0, 4, 8, 8, 0, 6, 0, 0, 0, 2, // 130
		    0, 0, 0, 0, 0, 6, 0, 0, 0, 0, // 140
		    0, 0, 1, 2, 0, 2, 6, 0, 0, 0, // 150
		    0, 0, 0, 0, -1, -1, 0, 0, 0, 0,// 160
		    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 170
		    0, 8, 0, 3, 0, 2, 0, 0, 8, 1, // 180
		    0, 0, 14, 0, 0, 0, 0, 0, 0, 0, // 190
		    2, 0, 0, 0, 0, 0, 0, 0, 4, 0, // 200
		    4, 0, 0, 0, 7, 8, 0, 0, 10, 0, // 210
		    0, 0, 0, 0, 0, 0, -1, 0, 6, 0, // 220
		    1, 0, 0, 0, 6, 0, 6, 8, 1, 0, // 230
		    0, 4, 0, 0, 0, 0, -1, 0, -1, 4,// 240
		    0, 0, 8, 6, 0, 0, 0 // 250
        };

    }
}
