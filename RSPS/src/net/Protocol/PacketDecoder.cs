using RSPS.src.entity.player;
using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        public readonly Player _player;


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
            StringBuilder debugSb = new();
            debugSb.AppendLine(">>> Decode packet [DataSize: " + reader.Data.Length + "][Pointer: " + reader.Pointer + "]");

            int packetOpCode = reader.ReadByte();
            packetOpCode = packetOpCode - connection.NetworkDecryptor.getNextValue() & 0xFF;
            int packetSize = PacketSizes[packetOpCode];

            debugSb.Append("Opcode: " + packetOpCode + ", Packetsize: " + packetSize);

            if (packetSize == -1)
            { // Resolve dynamic packet size
                if (reader.Pointer >= reader.Length)
                {
                    debugSb.AppendLine("Unable to determine dynamic packet size, no more data!");
                    debugSb.AppendLine("<<< -------------------------------");
                    Debug.WriteLine(debugSb.ToString());
                    return connection.ProtocolDecoder;
                }
                packetSize = reader.ReadByte();

                if (packetSize < 0)
                {
                    debugSb.AppendLine("Received invalid packet size, was dynamic but received " + packetSize);
                    debugSb.AppendLine("<<< -------------------------------");
                    Debug.WriteLine(debugSb.ToString());
                    return connection.ProtocolDecoder;
                }
                debugSb.Append(" (" + packetSize + ")");
            }
            debugSb.AppendLine();

            if (packetSize > 0 && (reader.Pointer + packetSize) > reader.Length)
            {
                debugSb.AppendLine("Invalid packet, expected " + packetSize + " payload but only found " + (reader.Length - reader.Pointer));
                //debugSb.AppendLine("<<< -------------------------------");
                //Debug.WriteLine(debugSb.ToString());
                //return connection.ProtocolDecoder;
            }
            if (packetSize <= 0 && reader.Pointer < reader.Length)
            {
                debugSb.AppendLine("Invalid packet, found payload (" + (reader.Length - reader.Pointer) + ") but none was expected");
                //debugSb.AppendLine("<<< -------------------------------");
                //Debug.WriteLine(debugSb.ToString());
                //return null;
            }
            try
            {
                if (reader.Length > packetSize)
                {
                    reader.Opcode = packetOpCode;
                    reader.PayloadSize = packetSize;

                    PacketHandler.HandlePacket(_player, reader);

                    while (reader.Length - reader.Pointer > 0)
                    {
                        reader.ReadByte();
                    }
                }
                debugSb.AppendLine("Handled packet (pointer: " + reader.Pointer + "), unread payload left: " + (reader.Length - reader.Pointer));
                debugSb.AppendLine("<<< -------------------------------");

                if (packetOpCode != 0)
                Debug.WriteLine(debugSb.ToString());
            }
            catch (Exception ex)
            {
                debugSb.AppendLine("Exception occurred while handling packet: " + ex.Message);
                debugSb.AppendLine("<<< -------------------------------");
                Debug.WriteLine(debugSb.ToString());
                return null;
            }
            


            /*
            while (reader.Pointer < reader.Length)
            { // Handle the received packet
              // Handle a packet for an existing connection
                try
                {
                    int packetOpCode = reader.ReadByte() & 0xFF;
                    packetOpCode = packetOpCode - connection.NetworkDecryptor.getNextValue() & 0xFF;
                    int packetSize = PacketSizes[packetOpCode];

                    if (packetSize == -1)//variable length packet
                    {
                        if (reader.Pointer >= reader.Length)
                        {
                            break;
                        }
                        packetSize = reader.ReadByte() & 0xFF;
                    }
                    if (reader.Length >= packetSize)
                    {
                        reader.Opcode = packetOpCode;
                        reader.PayloadSize = packetSize;

                        PacketHandler.HandlePacket(_player, reader);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return null;
                }
            }*/
            return new PacketDecoder(_player);
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
