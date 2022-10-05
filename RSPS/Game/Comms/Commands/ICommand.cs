using RSPS.Entities.Mobiles.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Comms.Commands
{
    /// <summary>
    /// Represents a command handler
    /// </summary>
    public interface ICommand
    {

        /// <summary>
        /// The command execution action taking the command arguments as parameter
        /// </summary>
        public Action<Player, string[]> Execute { get; set; }

        /// <summary>
        /// Whether input matches one of the command identfiers
        /// </summary>
        /// <param name="input">The input</param>
        /// <returns>The result</returns>
        public bool MatchesIdentifier(string input);

        /// <summary>
        /// Whether the required arguments are present for the command
        /// </summary>
        /// <param name="arguments">The arguments</param>
        /// <returns>The result</returns>
        public bool HasRequiredArguments(string[] arguments);

        /// <summary>
        /// Whether the executor has the required rights
        /// </summary>
        /// <param name="rights">The executor right</param>
        /// <returns>The result</returns>
        public bool HasRequiredRights(PlayerRights rights);

    }
}
