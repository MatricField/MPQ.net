using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MPQNet.Helper
{
    public static class Math
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2(uint x) => 32 - BitOperations.LeadingZeroCount(x) - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2(int x) => Log2((uint)x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2(ulong x) => 32 - BitOperations.LeadingZeroCount(x) - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Log2(long x) => Log2((ulong)x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPowerOf2<T>(T x)
            where T : IBinaryInteger<T>
        {
            return (x & (x -  T.One)) == T.Zero;
        }

        /// <summary>
        /// Combine high bits and low bits of offset data
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long CombineTo64(long hightBits, uint lowBits)
        {
            return hightBits << 32 | lowBits;
        }

        /// <summary>
        /// Combine high bits and low bits of offset data
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BreakTo32(long value, out uint hightBits, out uint lowBits)
        {
            lowBits = Convert.ToUInt32(value);
            hightBits = Convert.ToUInt32(value >> 32);
        }
    }
}
