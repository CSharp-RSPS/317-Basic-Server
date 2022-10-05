using RSPS.Entities.Mobiles.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Game.Comms.Commands
{
    /// <summary>
    /// Represents a player command
    /// </summary>
    public class Command : ICommand
    {

        /// <summary>
        /// The minimum rights required to execute the command
        /// </summary>
        public PlayerRights MinRights { get; private set; }

        /// <summary>
        /// The command identifiers
        /// </summary>
        public string[] Identifiers { get; private set; }

        /// <summary>
        /// The supported argument counts
        /// </summary>
        public int[]? SupportedArguments { get; private set; }

        public Action<Player, string[]> Execute { get; set; }


        /// <summary>
        /// Creates a new command
        /// </summary>
        /// <param name="minRights">The minimum rights required to execute the command</param>
        /// <param name="identifiers">The command identifiers</param>
        /// <param name="supportedArguments">The supported argument counts</param>
        public Command(PlayerRights minRights, string[] identifiers, int[]? supportedArguments = null)
        {
            MinRights = minRights;
            Identifiers = identifiers;
            SupportedArguments = supportedArguments;
        }

        public bool MatchesIdentifier(string input)
        {
            return Identifiers.Any(identifier => identifier.ToLower().Equals(input.ToLower()));
        }

        public bool HasRequiredArguments(string[] arguments)
        {
            if (SupportedArguments == null || SupportedArguments.Length == 0)
            {
                return true;
            }
            return SupportedArguments.Any(sa => sa == arguments.Length);
        }

        public bool HasRequiredRights(PlayerRights rights)
        {
            return rights >= MinRights;
        }
        
    }
}
