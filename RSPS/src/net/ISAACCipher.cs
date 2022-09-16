﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net
{
    /**
     * @author Graham Edgecombe
     **/
    public class ISAACCipher
    {

        /**
         * The golden ratio.
         */
        private static readonly uint RATIO = 0x9e3779b9;

        /**
         * The log of the size of the results and memory arrays.
         */
        private static readonly int SIZE_LOG = 8;

        /**
         * The size of the results and memory arrays.
         */
        private static readonly int SIZE = 1 << SIZE_LOG;

        /**
         * For pseudo-random lookup.
         */
        private static readonly int MASK = (SIZE - 1) << 2;

        /**
         * The count through the results.
         */
        private int count = 0;

        /**
         * The results.
         */
        private readonly int[] results = new int[SIZE];

        /**
         * The internal memory state.
         */
        private readonly int[] memory = new int[SIZE];

        /**
         * The accumulator.
         */
        private int a;

        /**
         * The last result.
         */
        private int b;

        /**
         * The counter.
         */
        private int c;

        public ISAACCipher(int[] seed)
        {
            for (int i = 0; i < seed.Length; i++)
            {
                results[i] = seed[i];
            }
            init(true);
        }

        /**
         * Gets the next value.
         *
         * @return The next value.
         */
        public int getNextValue()
        {
            if (count-- == 0)
            {
                isaac();
                count = SIZE - 1;
            }
            return results[count];
        }

        /**
         * Generates 256 results.
         */
        private void isaac()
        {
            int i, j, x, y;
            b += ++c;

            for (i = 0, j = SIZE / 2; i < SIZE / 2;)
            {
                x = memory[i];
                a ^= a << 13;
                a += memory[j++];
                memory[i] = y = memory[(x & MASK) >> 2] + a + b;
                results[i++] = b = memory[((y >> SIZE_LOG) & MASK) >> 2] + x;

                x = memory[i];
                a ^= (int)((uint)a >> 6);
                a += memory[j++];
                memory[i] = y = memory[(x & MASK) >> 2] + a + b;
                results[i++] = b = memory[((y >> SIZE_LOG) & MASK) >> 2] + x;

                x = memory[i];
                a ^= a << 2;
                a += memory[j++];
                memory[i] = y = memory[(x & MASK) >> 2] + a + b;
                results[i++] = b = memory[((y >> SIZE_LOG) & MASK) >> 2] + x;

                x = memory[i];
                a ^= (int)((uint)a >> 16);
                a += memory[j++];
                memory[i] = y = memory[(x & MASK) >> 2] + a + b;
                results[i++] = b = memory[((y >> SIZE_LOG) & MASK) >> 2] + x;
            }

            for (j = 0; j < SIZE / 2;)
            {
                x = memory[i];
                a ^= a << 13;
                a += memory[j++];
                memory[i] = y = memory[(x & MASK) >> 2] + a + b;
                results[i++] = b = memory[((y >> SIZE_LOG) & MASK) >> 2] + x;

                x = memory[i];
                a ^= (int)((uint)a >> 6);
                a += memory[j++];
                memory[i] = y = memory[(x & MASK) >> 2] + a + b;
                results[i++] = b = memory[((y >> SIZE_LOG) & MASK) >> 2] + x;

                x = memory[i];
                a ^= a << 2;
                a += memory[j++];
                memory[i] = y = memory[(x & MASK) >> 2] + a + b;
                results[i++] = b = memory[((y >> SIZE_LOG) & MASK) >> 2] + x;

                x = memory[i];
                a ^= (int)((uint)a >> 16);
                a += memory[j++];
                memory[i] = y = memory[(x & MASK) >> 2] + a + b;
                results[i++] = b = memory[((y >> SIZE_LOG) & MASK) >> 2] + x;
            }
        }

        /**
         * Initialises the ISAAC.
         *
         * @param secondPass Flag indicating if we should perform a second pass.
         */
        private void init(bool secondPass)
        {
            int i;
            int a, b, c, d, e, f, g, h;
            a = b = c = d = e = f = g = h = (int)RATIO;

            for (i = 0; i < 4; ++i)
            {
                a ^= b << 11;
                d += a;
                b += c;
                b ^= (int)((uint)c >> 2);
                e += b;
                c += d;
                c ^= d << 8;
                f += c;
                d += e;
                d ^= (int)((uint)e >> 16);
                g += d;
                e += f;
                e ^= f << 10;
                h += e;
                f += g;
                f ^= (int)((uint)g >> 4);
                a += f;
                g += h;
                g ^= h << 8;
                b += g;
                h += a;
                h ^= (int)((uint)a >> 9);
                c += h;
                a += b;
            }

            for (i = 0; i < SIZE; i += 8)
            {
                if (secondPass)
                {
                    a += results[i];
                    b += results[i + 1];
                    c += results[i + 2];
                    d += results[i + 3];
                    e += results[i + 4];
                    f += results[i + 5];
                    g += results[i + 6];
                    h += results[i + 7];
                }
                a ^= b << 11;
                d += a;
                b += c;
                b ^= (int)((uint)c >> 2);
                e += b;
                c += d;
                c ^= d << 8;
                f += c;
                d += e;
                d ^= (int)((uint)e >> 16);
                g += d;
                e += f;
                e ^= f << 10;
                h += e;
                f += g;
                f ^= (int)((uint)g >> 4);
                a += f;
                g += h;
                g ^= h << 8;
                b += g;
                h += a;
                h ^= (int)((uint)a >> 9);
                c += h;
                a += b;
                memory[i] = a;
                memory[i + 1] = b;
                memory[i + 2] = c;
                memory[i + 3] = d;
                memory[i + 4] = e;
                memory[i + 5] = f;
                memory[i + 6] = g;
                memory[i + 7] = h;
            }

            if (secondPass)
            {
                for (i = 0; i < SIZE; i += 8)
                {
                    a += memory[i];
                    b += memory[i + 1];
                    c += memory[i + 2];
                    d += memory[i + 3];
                    e += memory[i + 4];
                    f += memory[i + 5];
                    g += memory[i + 6];
                    h += memory[i + 7];
                    a ^= b << 11;
                    d += a;
                    b += c;
                    b ^= (int)((uint)c >> 2);
                    e += b;
                    c += d;
                    c ^= d << 8;
                    f += c;
                    d += e;
                    d ^= (int)((uint)e >> 16);
                    g += d;
                    e += f;
                    e ^= f << 10;
                    h += e;
                    f += g;
                    f ^= (int)((uint)g >> 4);
                    a += f;
                    g += h;
                    g ^= h << 8;
                    b += g;
                    h += a;
                    h ^= (int)((uint)a >> 9);
                    c += h;
                    a += b;
                    memory[i] = a;
                    memory[i + 1] = b;
                    memory[i + 2] = c;
                    memory[i + 3] = d;
                    memory[i + 4] = e;
                    memory[i + 5] = f;
                    memory[i + 6] = g;
                    memory[i + 7] = h;
                }
            }
            isaac();
            count = SIZE;
        }

    }
}
