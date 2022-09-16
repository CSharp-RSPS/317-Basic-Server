using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.player
{
    public class PublicChat
    {
        public readonly int color;
        public readonly int effects;
        public readonly byte[] text;

        public PublicChat(int color, int effects, byte[] text)
        {
            this.color = color;
            this.effects = effects;
            this.text = text;
        }

    }
}
