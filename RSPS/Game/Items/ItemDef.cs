using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Items
{
    /// <summary>
    /// Represents an item definition
    /// </summary>
    public sealed class ItemDef
    {

        public int Identity { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int ReverseIdentity { get; set; }

        public double Weight { get; set; }

        public bool Noted { get; set; }

        public bool Stackable { get; set; }

        public bool Member { get; set; }

        public bool Shareable { get; set; }

        public bool Enabled { get; set; }

    }
}
