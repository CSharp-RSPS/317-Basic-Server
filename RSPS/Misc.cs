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
        /// Holds the valid characters for player usernames
        /// </summary>
        private static readonly char[] ValidChars = { '_', 'a', 'b', 'c', 'd', 'e',
            'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
            's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4',
            '5', '6', '7', '8', '9', '!', '@', '#', '$', '%', '^', '&', '*',
            '(', ')', '-', '+', '=', ':', ';', '.', '>', '<', ',', '"', '[',
            ']', '|', '?', '/', '`' };

        /// <summary>
		/// Converts a string to a long value
		/// </summary>
		/// <param name="input">The string</param>
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
        /// Converts a long value to a sting
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The string</returns>
        public static string DecodeBase37(long value)
        {
            int i = 0;
            char[] ac = new char[12];

            while (value != 0L)
            {
                long l1 = value;
                value /= 37L;
                ac[11 - i++] = ValidChars[(int)(l1 - value * 37L)];
            }
            return new string(ac, 12 - i, i);
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
