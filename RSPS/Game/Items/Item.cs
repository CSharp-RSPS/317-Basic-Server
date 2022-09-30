using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items
{
    /// <summary>
    /// Represents a game item
    /// </summary>
    public class Item
    {

        /// <summary>
        /// The unique identifier of the item
        /// </summary>
        public string Uid { get; private set; }

        /// <summary>
        /// The item identifier
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The amount of the item
        /// </summary>
        public int Amount { get; private set; }


        /// <summary>
        /// Creates a new item
        /// </summary>
        /// <param name="id">The item identifier</param>
        /// <param name="amount">The amount of the item</param>
        public Item(int id, int amount = 1) : this(Guid.NewGuid().ToString(), id, amount) { }

        /// <summary>
        /// Creates a new item
        /// </summary>
        /// <param name="uid">The unique identifier</param>
        /// <param name="id">The item identifier</param>
        /// <param name="amount">The amount of the item</param>
        public Item(string uid, int id, int amount)
        {
            Uid = uid;
            Id = id;
            Amount = amount;
        }

        /// <summary>
        /// Copies the item
        /// </summary>
        /// <returns>The copied item</returns>
        public Item Copy()
        {
            return new Item(Id, Amount);
        }

    }
}
