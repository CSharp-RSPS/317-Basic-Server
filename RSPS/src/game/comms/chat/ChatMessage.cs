using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.game.comms.chat
{
    /// <summary>
    /// Represents a chat message
    /// </summary>
    public class ChatMessage
    {

        /// <summary>
        /// The effects
        /// </summary>
        public int Effects { get; private set; }

        /// <summary>
        /// The text color
        /// </summary>
        public int Color { get; private set; }

        /// <summary>
        /// The text
        /// </summary>
        public byte[] Text { get; private set; }


        /// <summary>
        /// Creates a new chat message
        /// </summary>
        /// <param name="effects">The effects</param>
        /// <param name="color">The text color</param>
        /// <param name="text">The text</param>
        public ChatMessage(int effects, int color, byte[] text)
        {
            Effects = effects;
            Color = color;
            Text = text;
        }

    }
}
