using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegerLibrary.Utilities
{
    /// <summary>
    /// From Bit Twiddling Hacks
    /// </summary>
    public static class BitTwiddlingExtensions
    {
        public static uint Interleave(this uint x, uint y)
        {
            uint z; // z gets the resulting 32-bit Morton Number.  
                    // x and y must initially be less than 65536.

            x = (x | x << 8) & 0x00FF00FF;
            x = (x | x << 4) & 0x0F0F0F0F;
            x = (x | x << 2) & 0x33333333;
            x = (x | x << 1) & 0x55555555;

            y = (y | y << 8) & 0x00FF00FF;
            y = (y | y << 4) & 0x0F0F0F0F;
            y = (y | y << 2) & 0x33333333;
            y = (y | y << 1) & 0x55555555;

            z = x | y << 1;
            return z;
        }
        public static long InterleaveZero2D(this int n)
        {
            long x = n;
            x = (x | x << 16) & 0x0000FFFF0000FFFF;
            x = (x | x << 8) & 0x00FF00FF00FF00FF;
            x = (x | x << 4) & 0x0F0F0F0F0F0F0F0F;
            x = (x | x << 2) & 0x3333333333333333;
            x = (x | x << 1) & 0x5555555555555555;
            return x;
        }
        public static long InterleaveZero3D(this int n)
        {
            // Mask unused bits
            long x = n;
            x = (x | x << 32) & 0x1F00000000FFFF; // 00011111000000000000000000000000000000001111111111111111
            x = (x | x << 16) & 0x1F0000FF0000FF; // 00011111000000000000000011111111000000000000000011111111
            x = (x | x << 8) & 0x100F00F00F00F00F; // 0001000000001111000000001111000000001111000000001111000000000000
            x = (x | x << 4) & 0x10C30C30C30C30C3; // 0001000011000011000011000011000011000011000011000011000100000000
            x = (x | x << 2) & 0x1249249249249249; //‭ 0001001001001001001001001001001001001001001001001001001001001001‬
            return x;
        }
        public static long Interleave2D(this int x, int y)
        {
            long result = 0;
            result |= x.InterleaveZero2D() | y.InterleaveZero2D() << 1;
            return result;
        }
        public static bool IsPowerOf2(int n)
        {
            return (n & n - 1) == 0;
        }
        public static uint RoundUpToNextPowerOf2(this uint v)
        {
            // compute the next highest power of 2 of 32-bit v
            v--;
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v++;
            return v;
        }
        public static uint MostSignificantBit(this uint v)
        {
            uint c = 32; // c will be the number of zero bits on the right
            v &= (uint)-Math.Sign(v);
            if (v != 0) c--;
            if ((v & 0x0000FFFF) != 0) c -= 16;
            if ((v & 0x00FF00FF) != 0) c -= 8;
            if ((v & 0x0F0F0F0F) != 0) c -= 4;
            if ((v & 0x33333333) != 0) c -= 2;
            if ((v & 0x55555555) != 0) c -= 1;
            return c;
        }
        public static uint BitMatch(this uint a, uint b)
        {
            uint result = 0;
            uint bitmask = (uint)1 << 31;
            for (uint n = 0; n < 31; n++)
            {
                if ((a & bitmask) != (b & bitmask))
                    return n;
                bitmask >>= 1;
            }
            return 0;
        }
        public static int PowerOf2(this int v)
        {
            // compute the next highest power of 2 of 32-bit v
            v--;
            v |= v >> 1;
            v |= v >> 2;
            v |= v >> 4;
            v |= v >> 8;
            v |= v >> 16;
            v++;
            return v;
        }
    }
}
