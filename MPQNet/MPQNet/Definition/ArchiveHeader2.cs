//MIT License

//Copyright(c) 2023 Mingxi "Lucien" Du

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

using MPQNet.Helper;

namespace MPQNet.Definition
{
    internal record class Header2 : Header1
    {
        protected ulong _ExtendedBlockTableOffset;

        /// <summary>
        /// Offset to the beginning of array of 16-bit high parts of file offsets.
        /// </summary>
        public required virtual ulong ExtendedBlockTableOffset
        {
            get => _ExtendedBlockTableOffset;
            init => _ExtendedBlockTableOffset = value;
        }

        /// <summary>
        /// High 16 bits of the hash table offset for large archives.
        /// </summary>
        protected ushort _HashTableOffsetHigh;

        public required override long HashTableOffset
        {
            get => Math.CombineTo64(_HashTableOffsetHigh, _HashTableOffset);
            init {
                Math.BreakTo32(value, out var hightBits, out _HashTableOffset);
                _HashTableOffsetHigh = (ushort)hightBits;
            }
        }

        /// <summary>
        /// High 16 bits of the block table offset for large archives.
        /// </summary>
        protected ushort _BlockTableOffsetHigh;

        public required override long BlockTableOffset
        {
            get => Math.CombineTo64(_BlockTableOffsetHigh, _BlockTableOffset);
            init
            {
                Math.BreakTo32(value, out var hightBits, out _BlockTableOffset);
                _BlockTableOffsetHigh = (ushort)hightBits;
            }
        }

        public Header2()
            :base()
        {

        }

        public Header2(in RawHeader header)
            : base(header)
        {
            _ExtendedBlockTableOffset = header.HiBlockTablePos64;
        }
    }
}
