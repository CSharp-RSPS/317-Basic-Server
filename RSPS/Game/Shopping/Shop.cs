using RSPS.Game.Items.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Shopping
{
    public class Shop
    {

        /// <summary>
        /// The store inventory
        /// </summary>
        public ItemContainer Inventory { get; private set; }


        public Shop()
        {
            Inventory = new ItemContainer(ItemContainerType.Shop, true);
        }

    }
}
