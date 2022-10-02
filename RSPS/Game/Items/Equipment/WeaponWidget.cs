using RSPS.Game.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items.Equipment
{
    /// <summary>
    /// Represents a widget for items worn in the equipment weapon slot
    /// </summary>
    public sealed class WeaponWidget
    {

        /// <summary>
        /// The button ID activating this widget
        /// </summary>
        public int ButtonId { get; set; }

        /// <summary>
        /// The fighting type
        /// </summary>
        public FightType FightType { get; set; }

        /// <summary>
        /// The fighting style
        /// </summary>
        public FightStyle FightStyle { get; set; }

        /// <summary>
        /// The config value representing the order of the widgets
        /// </summary>
        public int ConfigValue { get; set; }


        /// <summary>
        /// Creates a new weapon widget
        /// </summary>
        /// <param name="buttonId">The button ID</param>
        /// <param name="fightType">The fighting type</param>
        /// <param name="fightStyle">the fighting style</param>
        /// <param name="configValue">The config value representing the order of the widgets</param>
        public WeaponWidget(int buttonId, FightType fightType, FightStyle fightStyle, int configValue)
        {
            ButtonId = buttonId;
            FightType = fightType;
            FightStyle = fightStyle;
            ConfigValue = configValue;
        }
    }
}
