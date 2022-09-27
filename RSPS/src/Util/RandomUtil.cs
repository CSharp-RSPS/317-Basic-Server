using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.Util
{
    /// <summary>
    /// Contains randomization related utilities
    /// </summary>
    public static class RandomUtil
    {


        /// <summary>
        /// Retrieves a random integer value between zero and a given value
        /// </summary>
        /// <param name="maxInclusive">The max. value (inclusive)</param>
        /// <returns>The result</returns>
        public static int RandomInt(int maxInclusive)
        {
            return new Random().Next(0, maxInclusive + 1);
        }

        /// <summary>
        /// Retrieves a random integer value
        /// </summary>
        /// <param name="minInclusive">The min. value (inclusive)</param>
        /// <param name="maxInclusive">The max. value (inclusive)</param>
        /// <returns>The result</returns>
        public static int RandomInt(int minInclusive, int maxInclusive)
        {
            return new Random().Next(minInclusive, maxInclusive + 1);
        }

        /// <summary>
        /// Retrieves a random percentage
        /// </summary>
        /// <returns>The result</returns>
        public static int GetPercentage()
        {
            return RandomInt(0, 100);
        }

    }
}
