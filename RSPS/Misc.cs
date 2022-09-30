using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS
{
    /// <summary>
    /// Contains various methods and utilities
    /// </summary>
    public static class Misc
    {

        /// <summary>
		/// Converts a string to a long value
		/// </summary>
		/// <param name="input"></param>
		/// <returns>The long value</returns>
        public static long EncodeBase37(string input)
        {
            long l = 0L;

            for (int i = 0; i < input.Length && i < 12; i++)
            {
                char c = input.ToCharArray(i, 1)[0];
                l *= 37L;

                if (c >= 'A' && c <= 'Z')
                    l += 1 + c - 65;
                else if (c >= 'a' && c <= 'z')
                    l += 1 + c - 97;
                else if (c >= '0' && c <= '9')
                    l += 27 + c - 48;
            }

            while (l % 37L == 0L && l != 0L)
                l /= 37L;
            return l;
        }

        /// <summary>
        /// Converts a hex value in bytes to an integer
        /// </summary>
        /// <param name="data">The hex byte data</param>
        /// <returns>the integer</returns>
        public static int HexToInt(byte[] data)
        {
            int value = 0;
            int n = 1000;

            for (int i = 0; i < data.Length; i++)
            {
                int num = (data[i] & 0xFF) * n;
                value += num;

                if (n > 1)
                {
                    n /= 1000;
                }
            }
            return value;
        }

    }
}
