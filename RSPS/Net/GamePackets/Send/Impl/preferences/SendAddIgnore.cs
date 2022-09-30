using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Sends the IDs of all the users that this player has in their ignore.
    /// Note: By looking at the rest of the 317 protocol, 
    /// there doesn't seem to be a way to change the list dynamically. 
    /// It seems as though that whenever the player decides to add or remove a player from their list, it must send all the values again.
    /// </summary>
    [PacketDef(PacketDefinition.SendAddIgnore)]
    public sealed class SendAddIgnore : IPacketVariablePayloadBuilder
    {

        /// <summary>
        /// The player name as long block
        /// </summary>
        public long PlayerNameBlock { get; private set; }


        /// <summary>
        /// Creates a new add ignore packet payload builder
        /// </summary>
        /// <param name="playerNameBlock">The player name as long block</param>
        public SendAddIgnore(long playerNameBlock)
        {
            PlayerNameBlock = playerNameBlock;
        }

        public int GetPayloadSize()
        {
            throw new NotImplementedException();
        }

        public void WritePayload(PacketWriter writer)
        {
            /*
            int entries = packetSize / 8;

            for (int i = 0; i < entries; i++)
            {
                ignoreList[i] = stream.readLong();
            }*/
            writer.WriteLong(PlayerNameBlock);
        }

    }

}
