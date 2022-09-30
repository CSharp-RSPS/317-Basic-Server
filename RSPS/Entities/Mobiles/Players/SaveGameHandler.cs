using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Entities.Mobiles.Players
{
    public static class SaveGameHandler
    {


        /// <summary>
        /// Retrieves whether a save game for a given username exists
        /// </summary>
        /// <param name="username">The username</param>
        /// <returns>The result</returns>
        public static bool SaveGameExists(string username)
        {
            return false;
        }

        /// <summary>
        /// Verifies the password of a loaded player
        /// </summary>
        /// <param name="password">The password to verify</param>
        /// <param name="passwordToMatch">The password that should match</param>
        /// <returns>The result</returns>
        public static bool VerifyPassword(string password, string passwordToMatch)
        {
            return password.Equals(passwordToMatch);
        }

        /// <summary>
        /// Saves the game for a player
        /// </summary>
        /// <param name="player">The player</param>
        public static void SaveGame(Player player)
        {

        }

        /// <summary>
        /// Loads the save game for a player
        /// </summary>
        /// <param name="player">The player</param>
        /// <returns>Whether loading the save game was successful</returns>
        public static bool LoadSaveGame(Player player)
        {

            return true;
        }

    }
}
