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

        private int packetOpcode;

        private int packetPayloadSize;

        private byte[]? packetPayloadBuffer;

        private int packetsHandled = 0;


        /// <summary>
        /// Creates a new protocol decoder
        /// </summary>
        /// <param name="player">The player we're decoding packets for</param>
        public PacketDecoder(Player player)
        {
            _player = player;
            packetOpcode = -1;
            packetPayloadSize = -1;
        }

        public bool Decode(Connection connection, PacketReader reader)
        {
            if (connection.NetworkDecryptor == null)
            {
                Console.Error.WriteLine("Unable to decode packet as no decryptor is present");
                return false;
            }
            Debug.WriteLine("Buffer: " + string.Join(" ", reader.Buffer.ToArray()));

            if (reader.ReadableBytes <= 0 || connection.ConnectionState == ConnectionState.Disconnected)
            {
                return connection.ConnectionState != ConnectionState.Disconnected;
            }
            if (packetOpcode == -1)
            { // Extract the packet opcode and the corresponding default size
                packetOpcode = reader.ReadByte();
                packetOpcode = packetOpcode - connection.NetworkDecryptor.getNextValue() & 0xFF;
                packetPayloadSize = PacketSizes[packetOpcode];
                packetPayloadBuffer = packetPayloadSize != -1 ? new byte[packetPayloadSize] : null;
            }
            if (packetPayloadBuffer == null)
            { // Resolve the packet size of a dynamic packet
                if (reader.ReadableBytes <= 0)
                { // End of data stream reached
                    Debug.WriteLine("Unable to determine variable packet size for packet {0}, no more data present", packetOpcode);
                    return true;
                }
                packetPayloadSize = reader.ReadByte();
                packetPayloadBuffer = new byte[packetPayloadSize];
            }
            if (reader.ReadableBytes >= packetPayloadSize)
            { // Retrieve the payload of the packet
                Array.Copy(reader.Buffer, reader.Pointer, packetPayloadBuffer, 0, packetPayloadSize);
                Debug.WriteLine("Copied packet payload to buffer, reader pointing @ {0}/{1}", reader.Pointer, reader.Length);
                reader.Pointer += packetPayloadSize;

                try
                {
                    PacketHandler.HandlePacket(_player, packetOpcode, packetPayloadSize, new(packetPayloadBuffer));
                    packetsHandled++;

                    Debug.WriteLine(">>> Packet {0} with size {1} built. Payload => {2}", 
                        packetOpcode, packetPayloadSize, string.Join(" ", packetPayloadBuffer));
                    Debug.WriteLine("--------------------------------------");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return false;
                }
                finally
                {
                    packetOpcode = -1;
                    packetPayloadSize = -1;
                }
                if (reader.ReadableBytes > 0)
                { // If there is still data left after handling a packet we continue decoding for a potential next packet
                    return Decode(connection, reader);
                }
            }
            /*
            while (reader.ReadableBytes > 0 && connection.ConnectionState != ConnectionState.Disconnected)
            { // As long as there's data left in the data buffer and the connection is connected we try to consume the data
              

                
                

                if (reader.ReadableBytes >= packetSize)
                { // Retrieve the packet payload
                    Array.Copy(reader.Buffer, reader.Pointer, buffer, 0, packetSize);
                    reader.Pointer += packetSize;
                    Debug.WriteLine("Copied packet payload to buffer, reader pointing @ {0}/{1}", reader.Pointer, reader.Length);

                    try
                    {
                        PacketHandler.HandlePacket(_player, packetOpcode, packetSize, new(buffer));
                        packetsHandled++;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        return false;
                    }
                    reader.Pointer += packetSize;
                }

                Debug.WriteLine(">>> Packet {0} with size {1} built. Payload => {2}", packetOpcode, packetSize, string.Join(" ", buffer));
                Debug.WriteLine("--------------------------------------");
               
            }*/
            Debug.WriteLine("Handled {0} packets in decoding session. Reader data left: {1}", packetsHandled, reader.ReadableBytes);
            Debug.WriteLine(" =========================================== ");
            packetsHandled = 0;
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
