using RSPS.Entities.Mobiles.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Comms.Dialogues
{
    /// <summary>
    /// Represents a custom dialogue
    /// </summary>
    public abstract class CustomDialogue : Dialogue
    {


        /// <summary>
        /// Creates a new dialogue
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <param name="nextId">The follow-up dialogue identifier</param>
        /// <param name="expression">The expression of the speaker</param>
        /// <param name="lines">The text lines</param>
        protected CustomDialogue(int id, int nextId, DialogueExpression expression, string[] lines) 
            : base(id, nextId, expression, lines) { }

        /// <summary>
        /// Executes the custom dialogue
        /// </summary>
        /// <param name="player">The player</param>
        public abstract void Execute(Player player);

    }
}
