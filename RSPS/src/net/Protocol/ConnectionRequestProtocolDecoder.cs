using RSPS.src.net.Connections;
using RSPS.src.net.packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.Codec
{
    /// <summary>
    /// Represents a connection request decoder based on the RS2 protocol
    /// </summary>
    public sealed class ConnectionRequestProtocolDecoder : IProtocolDecoder
    {


        public bool Decode(Connection connection, PacketReader reader)
        {
            int connectionType = reader.ReadByte() & 0xff;

            switch (connectionType)
            {
                case 14: // Login request
                    break;

                case 15: // Update
                    return false;

                case 16: //New connection login
                    return false;

                case 18: //Reconnecting login
                    return false;

                default: //Unknown
                    return false;
            }
            int nameHash = reader.ReadByte() & 0xff; // used for login servers, not sure how it works

            connection.ConnectionState = ConnectionState.Authenticate;

            PacketWriter writer = Packet.CreatePacketWriter(17);
            writer.WriteLong(0);
            writer.WriteByte(0);
            writer.WriteLong(new Random().NextInt64());
            connection.Send(writer.Payload);

            return true;
        }


    }
}
