using RSPS.src.entity.player;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        /// The player we're decoding packets for
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

        public IProtocolDecoder? Decode(Connection connection, PacketReader reader)
        {
            if (connection.NetworkDecryptor == null)
            {
                Console.Error.WriteLine("Unable to decode packet as no decryptor is present");
                return null;
            }
            try
            {
                // Retrieve the opcode and assumed size of the received packet
                int packetOpCode = reader.ReadByte();
                packetOpCode = packetOpCode - connection.NetworkDecryptor.getNextValue() & 0xFF;
                int packetSize = PacketSizes[packetOpCode];

                if (packetSize == -1)//variable length packet
                { // Retrieve the packet size for a dynamic packet
                    if (reader.Pointer >= reader.Length)
                    {
                        return connection.ProtocolDecoder;
                    }
                    packetSize = reader.ReadByte();
                    Debug.WriteLine(packetOpCode + " => VARIABLE => " + packetSize);
                }else
                    Debug.WriteLine(packetOpCode + " => STATIC => " + packetSize);

                
                if (reader.ReadableBytes >= packetSize)
                { // If there is at least the size of our packet in data present we can handle the packet
                    // Create a buffer for the packet payload
                    byte[] payload = new byte[packetSize];
                    Array.Copy(reader.Buffer, reader.Pointer, payload, 0, payload.Length);
                    // Handle the packet
                    PacketHandler.HandlePacket(_player, new(payload)
                    {
                        Opcode = packetOpCode,
                        PayloadSize = packetSize
                    });
                    // Consume the packet size of the packet we handled
                    reader.ReadBytes(packetSize);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        //}
            return connection.ProtocolDecoder;
        }

        public static readonly int[] PacketSizes = {
            0, 0, 0, 1, -1, 0, 0, 0, 0, 0, // 0
            0, 0, 0, 0, 8, 0, 6, 2, 2, 0, // 10
            0, 2, 0, 6, 0, 12, 0, 0, 0, 0, // 20
            0, 0, 0, 0, 0, 8, 4, 0, 0, 2, // 30
            2, 6, 0, 6, 0, -1, 0, 0, 0, 0, // 40
            0, 0, 0, 12, 0, 0, 0, 0, 8, 0, // 50
            0, 8, 0, 0, 0, 0, 0, 0, 0, 0, // 60
            6, 0, 2, 2, 8, 6, 0, -1, 0, 6, // 70
            0, 0, 0, 0, 0, 1, 4, 6, 0, 0, // 80
            0, 0, 0, 0, 0, 3, 0, 0, -1, 0, // 90
            0, 13, 0, -1, 0, 0, 0, 0, 0, 0,// 100
            0, 0, 0, 0, 0, 0, 0, 6, 0, 0, // 110
            1, 0, 6, 0, 0, 0, -1, 0, 2, 6, // 120
            0, 4, 6, 8, 0, 6, 0, 0, 0, 2, // 130
            0, 0, 0, 0, 0, 6, 0, 0, 0, 0, // 140
            0, 0, 1, 2, 0, 2, 6, 0, 0, 0, // 150
            0, 0, 0, 0, - 1, -1, 0, 0, 0, 0,// 160
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 170
            0, 8, 0, 3, 0, 2, 0, 0, 8, 1, // 180
            0, 0, 12, 0, 0, 0, 0, 0, 0, 0, // 190
            2, 0, 0, 0, 0, 0, 0, 0, 4, 0, // 200
            4, 0, 0, 0, 7, 8, 0, 0, 10, 0, // 210
            0, 0, 0, 0, 0, 0, -1, 0, 6, 0, // 220
            1, 0, 0, 0, 6, 0, 6, 8, 1, 0, // 230
            0, 4, 0, 0, 0, 0, -1, 0, -1, 4,// 240
            0, 0, 6, 6, 0, 0, 0 // 250
        };

    }
}
