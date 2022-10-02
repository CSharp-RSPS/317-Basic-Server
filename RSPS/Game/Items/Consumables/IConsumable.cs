using RSPS.Entities.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items.Consumables
{
    /// <summary>
    /// Represents a consumable
    /// </summary>
    public interface IConsumable
    {


        /// <summary>
        /// Makes a mobile consume an item
        /// </summary>
        /// <typeparam name="T">The type of mobile</typeparam>
        /// <param name="mob">The mobile</param>
        /// <returns>Whether consumation was successful</returns>
        public bool Consume<T>(T mob) where T : Mobile;

    }
}
