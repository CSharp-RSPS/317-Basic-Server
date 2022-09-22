using RSPS.src.entity.player;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public readonly Player _player;


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
            int loop = 0;
            Debug.WriteLine("Buffer: " + string.Join(" ", reader.Buffer.ToArray()));

            while (reader.Pointer < reader.Length)
            {
                int startPointer = reader.Pointer;
                // Retrieve the opcode and assumed size of the received packet
                int packetOpcode = reader.ReadByte();
                packetOpcode = packetOpcode - connection.NetworkDecryptor.getNextValue() & 0xFF;
                int packetSize = PacketSizes[packetOpcode];

                if (packetSize == -1)//variable length packet
                { // Retrieve the packet size for a dynamic packet
                    if (reader.Pointer >= reader.Length)
                    {
                        break;
                    }
                    packetSize = reader.ReadByte() & 0xFF;

                    Debug.WriteLine(packetOpcode + " => VARIABLE => " + packetSize);
                }
                else
                    Debug.WriteLine(packetOpcode + " => STATIC => " + packetSize);

                if (reader.Length >= packetSize)
                {
                    //reader.Opcode = packetOpCode;
                  //  reader.PayloadSize = packetSize;

                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        return false;
                    }
                    PacketHandler.HandlePacket(_player, packetOpcode, packetSize, reader);
                    
                    Debug.WriteLine("[Loop: " + (++loop)+ "][Pointer: start@" + startPointer + " stop@" + reader.Pointer + "]" +
                        "[Opcode: " + packetOpcode + "][Size: " + packetSize + "]: " + string.Join(" ", reader.Buffer.ToArray()));
                    Debug.WriteLine("==========================");

                    //PacketHandler.HandlePacket(_player, reader);
                }
            }
            return true;
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
