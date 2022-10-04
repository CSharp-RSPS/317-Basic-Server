using RSPS.Game.UI;
using RSPS.Util.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items.Containers
{
    /// <summary>
    /// The possible item container types
    /// </summary>
    public enum ItemContainerType
    {

        [ItemContainerType(Interfaces.Inventory, -1, false, 28)]
        Inventory = 0,

        [ItemContainerType(Interfaces.Bank, Interfaces.InventoryOverlayBank, true, 68, 352)]
        Bank = 1,

        [ItemContainerType(Interfaces.Trade, Interfaces.InventoryOverlayTrade, false, 28)]
        Trade = 2,

        [ItemContainerType(Interfaces.Shop, Interfaces.InventoryOverlayShop, false, 36)]
        Shop = 3,

        [ItemContainerType(Interfaces.Equipment, -1, false, 14)]
        Equipment = 4

    }
}
