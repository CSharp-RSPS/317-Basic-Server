using RSPS.Entities.Mobiles.Players;
using RSPS.Game.Items.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Trading
{
    /// <summary>
    /// Handles trading related operations
    /// </summary>
    public static class TradingHandler
    {


        public static void Temp(Player player)
        {
            ItemContainer tradeContainer = new ItemContainer(3322, 28, player.PersistentVars.Member, false);
        }

    }
}
