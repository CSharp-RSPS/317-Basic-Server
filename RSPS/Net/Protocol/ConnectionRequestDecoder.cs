using RSPS.Net.Connections;
using RSPS.Net.GamePackets;
using RSPS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.Codec
{
    /// <summary>
    /// Represents a connection request decoder based on the RS2 protocol
    /// </summary>
    public sealed class ConnectionRequestDecoder : IProtocolDecoder
    {


        public bool Decode(Connection connection, PacketReader reader)
        {
            int connectionType = reader.ReadByte();

            switch (connectionType)
            {
                case 14: // Login request
                    break;

                case 15: // Update
                    break;

                case 16: //New connection login
                    return false;

                case 18: //Reconnecting login
                    return false;

                default: //Unknown
                    return false;
            }
            int nameHash = reader.ReadByte(); // nameHash: used for login servers, not sure how it works

            PacketWriter writer = new(17);
            writer.WriteLong(0);
            writer.WriteByte(0);
            writer.WriteLong(new Random().NextInt64()); // 8 ignored bytes
            connection.Send(writer);

            connection.ConnectionState = ConnectionState.Authenticate;

            return true;
        }


    }
}
