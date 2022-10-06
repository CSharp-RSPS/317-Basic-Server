using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.GamePackets.Send.Impl
{
    /// <summary>
    /// Spawn ground item for all except specified player
    /// </summary>
    [PacketDef(PacketDefinition.SpawnGroundItemForAllExceptSpecifiedPlayer)]
    public sealed class SendSpawnGroundItemForAllExceptPlayer : IPacketPayloadBuilder
    {

        /// <summary>
        /// The item identifier
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// The item quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// The player index
        /// </summary>
        public int PlayerIndex { get; set; }


        /// <summary>
        /// Spawns a ground item for all players except a specified player
        /// </summary>
        /// <param name="itemId">The item identifier</param>
        /// <param name="quantity">The item quantity</param>
        /// <param name="playerIndex">The player index</param>
        public SendSpawnGroundItemForAllExceptPlayer(int itemId, int quantity, int playerIndex)
        {
            ItemId = itemId;
            Quantity = quantity;
            PlayerIndex = playerIndex;
        }

        public void WritePayload(PacketWriter writer)
        {
            writer.WriteShortAdditional(ItemId);
            writer.WriteByteSubtrahend(0); // position offset?
            writer.WriteShortAdditional(PlayerIndex);
            writer.WriteShort(Quantity);

        }

    }

}
