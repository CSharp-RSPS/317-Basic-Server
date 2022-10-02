using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items
{
    /// <summary>
    /// Holds pricing information for an item
    /// </summary>
    public sealed class ItemPrizes
    {

        /// <summary>
        /// The item identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The default shop price
        /// </summary>
        public int ShopPrice { get; set; }

        /// <summary>
        /// The high alchemy value
        /// </summary>
        public int HighAlch { get; set; }

        /// <summary>
        /// The low alchemy value
        /// </summary>
        public int LowAlch { get; set; }

    }
}
