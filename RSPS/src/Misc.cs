using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src
{
    public class Misc
    {

        /**
         * Converts the username to a long value.
         */
        public static long EncodeBase37(String name)
        {
            long l = 0L;

            for (int i = 0; i < name.Length && i < 12; i++)
            {
                char c = name.ToCharArray(i, 1)[0];
                l *= 37L;

                if (c >= 'A' && c <= 'Z')
                    l += (1 + c) - 65;
                else if (c >= 'a' && c <= 'z')
                    l += (1 + c) - 97;
                else if (c >= '0' && c <= '9')
                    l += (27 + c) - 48;
            }

            while (l % 37L == 0L && l != 0L)
                l /= 37L;
            return l;
        }

        /**
         * Difference in X coordinates for directions array.
         */
        public static readonly sbyte[] DIRECTION_DELTA_X = new sbyte[] { -1, 0, 1, -1, 1, -1, 0, 1 };

        /**
         * Difference in Y coordinates for directions array.
         */
        public static readonly sbyte[] DIRECTION_DELTA_Y = new sbyte[] { 1, 1, 1, 0, 0, -1, -1, -1 };

		/**
		 * Calculates the direction between the two coordinates.
		 * 
		 * @param dx
		 *            the first coordinate
		 * @param dy
		 *            the second coordinate
		 * @return the direction
		 */
		public static int Direction(int dx, int dy)
		{
			if (dx < 0)
			{
				if (dy < 0)
				{
					return 5;
				}
				else if (dy > 0)
				{
					return 0;
				}
				else
				{
					return 3;
				}
			}
			else if (dx > 0)
			{
				if (dy < 0)
				{
					return 7;
				}
				else if (dy > 0)
				{
					return 2;
				}
				else
				{
					return 4;
				}
			}
			else
			{
				if (dy < 0)
				{
					return 6;
				}
				else if (dy > 0)
				{
					return 1;
				}
				else
				{
					return -1;
				}
			}
		}
		public static int hexToInt(byte[] data)
		{
			int value = 0;
			int n = 1000;
			for (int i = 0; i < data.Length; i++)
			{
				int num = (data[i] & 0xFF) * n;
				value += (int)num;
				if (n > 1)
				{
					n = n / 1000;
				}
			}
			return value;
		}

	}
}
