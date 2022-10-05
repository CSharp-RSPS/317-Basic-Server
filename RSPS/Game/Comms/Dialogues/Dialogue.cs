using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Comms.Dialogues
{
    /// <summary>
    /// Represents a dialogue
    /// </summary>
    public class Dialogue
    {

        /// <summary>
        /// The identifier
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The follow-up dialogue identifier
        /// </summary>
        public int NextId { get; private set; }

        /// <summary>
        /// The expression of the speaker
        /// </summary>
        public DialogueExpression? Expression { get; private set; }

        /// <summary>
        /// The text lines
        /// </summary>
        public string[] Lines { get; private set; }


        /// <summary>
        /// Creates a new dialogue
        /// </summary>
        /// <param name="id">The identifier</param>
        /// <param name="nextId">The follow-up dialogue identifier</param>
        /// <param name="expression">The expression of the speaker</param>
        /// <param name="lines">The text lines</param>
        public Dialogue(int id, int nextId, DialogueExpression? expression, string[] lines)
        {
            Id = id;
            NextId = nextId;
            Expression = expression;
            Lines = lines;
        }
    }
}
