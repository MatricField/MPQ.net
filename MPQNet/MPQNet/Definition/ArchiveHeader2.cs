//MIT License

//Copyright(c) 2018 Mingxi "Lucien" Du

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MPQNet.Definition
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class ArchiveHeader2 : ArchiveHeader
    {
        /// <summary>
        /// Combine high bits and low bits of offset data
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static long MakeOffset64(long hightBits, uint lowBits)
        {
            return hightBits << 32 | lowBits;
        }

        /// <summary>
        /// Offset to the beginning of array of 16-bit high parts of file offsets.
        /// </summary>
        public ulong ExtendedBlockTableOffset { get; }

        /// <summary>
        /// High 16 bits of the hash table offset for large archives.
        /// </summary>
        private readonly ushort HashTableOffsetHigh;

        public override long HashTableOffset => MakeOffset64(HashTableOffsetHigh, (uint)base.HashTableOffset);

        /// <summary>
        /// High 16 bits of the block table offset for large archives.
        /// </summary>
        private readonly ushort BlockTableOffsetHigh;

        public override long BlockTableOffset => MakeOffset64(BlockTableOffsetHigh, (uint)base.BlockTableOffset);
    }
}
