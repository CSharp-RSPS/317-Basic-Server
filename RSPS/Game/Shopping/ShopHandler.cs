using RSPS.Entities.Mobiles.Players;
using RSPS.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Shopping
{
    /// <summary>
    /// Handles shop related operations
    /// </summary>
    public static class ShopHandler
    {


        public static bool IsShopping(Player player)
        {
            return player.NonPersistentVars.OpenInterfaceId == Interfaces.Shop;
        }

    }
}
