using RSPS.src.entity.flag;
using RSPS.src.entity.updating;
using RSPS.src.game.comms.chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.entity.player.updating.impl
{
    /// <summary>
    /// Represents a chat state update
    /// </summary>
    public sealed class ChatStateUpdate : StateUpdate
    {

        /// <summary>
        /// The chat message
        /// </summary>
        public ChatMessage ChatMessage { get; private set; }


        /// <summary>
        /// Created a new chat state update
        /// </summary>
        /// <param name="chatMessage">The chat message</param>
        public ChatStateUpdate(ChatMessage chatMessage) : base(FlagType.Chat)
        {
            ChatMessage = chatMessage;
        }

    }
}
