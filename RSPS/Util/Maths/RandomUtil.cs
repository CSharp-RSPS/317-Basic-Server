using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Util.Maths
{
    /// <summary>
    /// Contains randomization related utilities
    /// </summary>
    public static class RandomUtil
    {

        /// <summary>
        /// The random number generator
        /// </summary>
        private static readonly Random Random = new();

        /// <summary>
        /// Retrieves a random integer value between zero and a given value
        /// </summary>
        /// <param name="maxInclusive">The max. value (inclusive)</param>
        /// <returns>The result</returns>
        public static int RandomInt(int maxInclusive)
        {
            return Random.Next(0, maxInclusive + 1);
        }

        /// <summary>
        /// Retrieves a random integer value
        /// </summary>
        /// <param name="minInclusive">The min. value (inclusive)</param>
        /// <param name="maxInclusive">The max. value (inclusive)</param>
        /// <returns>The result</returns>
        public static int RandomInt(int minInclusive, int maxInclusive)
        {
            return Random.Next(minInclusive, maxInclusive + 1);
        }

        /// <summary>
        /// Retrieves a random percentage
        /// </summary>
        /// <returns>The result</returns>
        public static int GetPercentage()
        {
            return RandomInt(0, 100);
        }

        /// <summary>
        /// Retrieves a random long value between zero and a given value
        /// </summary>
        /// <param name="maxInclusive">The max. value (inclusive)</param>
        /// <returns>The result</returns>
        public static long RandomLong(long maxInclusive)
        {
            return RandomLong(0, maxInclusive);
        }

        /// <summary>
        /// Retrieves a random long value
        /// </summary>
        /// <param name="minInclusive">The min. value (inclusive)</param>
        /// <param name="maxInclusive">The max. value (inclusive)</param>
        /// <returns>The result</returns>
        public static long RandomLong(long minInclusive, long maxInclusive)
        {
            return Random.NextInt64(minInclusive, maxInclusive + 1);
        }

        /// <summary>
        /// Retrieves a random double value between zero and a given value
        /// </summary>
        /// <param name="maxInclusive">The max. value (inclusive)</param>
        /// <returns>The result</returns>
        public static double RandomDouble(double maxExclusive = 1.0d)
        {
            return RandomDouble(0, maxExclusive);
        }

        /// <summary>
        /// Retrieves a random double value
        /// </summary>
        /// <param name="minInclusive">The min. value (inclusive)</param>
        /// <param name="maxInclusive">The max. value (inclusive)</param>
        /// <returns>The result</returns>
        public static double RandomDouble(double minInclusive, double maxExclusive)
        {
            return Random.NextDouble() * (maxExclusive - minInclusive) + minInclusive;
        }

    }
}
