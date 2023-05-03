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

using System.Diagnostics.CodeAnalysis;
using Math = MPQNet.Helper.Math;

namespace MPQNet.Definition
{
    internal record class Header1 : Header
    {
        protected ushort _SectorSizeShift;
        protected uint _HashTableOffset;
        protected uint _BlockTableOffset;
        protected uint _HashTableEntriesCount;
        protected uint _BlockTableEntriesCount;

        /// <summary>
        /// Power of two exponent specifying the number of 512-byte 
        /// disk sectors in each logical sector in the archive.
        /// The size of each logical sector in the archive is
        /// 512 * 2^SectorSizeShift.
        /// </summary>
        public required virtual long SectorSize
        {
            get => 512 << _SectorSizeShift;

            init
            {
                var (div, rem) = System.Math.DivRem(value, 512);
                if (0 == rem && Math.IsPowerOf2(div))
                {
                    _SectorSizeShift = checked((ushort)Math.Log2(div));
                }
            }
        }

        /// <summary>
        /// Offset to the beginning of the hash table,
        /// relative to the beginning of the archive.
        /// </summary>
        public required virtual long HashTableOffset
        {
            get => _HashTableOffset;
            init => _HashTableOffset = checked((uint)value);
        }

        /// <summary>
        /// Offset to the beginning of the block table,
        /// relative to the beginning of the archive.
        /// </summary>
        public required virtual long BlockTableOffset
        {
            get => _BlockTableOffset;
            init => _BlockTableOffset = checked((uint)value);
        }

        /// <summary>
        /// Number of entries in the hash table.
        /// </summary>
        public required virtual uint HashTableEntriesCount
        {
            get => _HashTableEntriesCount;
            init => _HashTableEntriesCount = value;
        }

        /// <summary>
        /// Number of entries in the block table.
        /// </summary>
        public required virtual uint BlockTableEntriesCount
        {
            get => _BlockTableEntriesCount;
            init => _BlockTableEntriesCount = value;
        }

        public Header1()
        {

        }

        [SetsRequiredMembers]
        public Header1(in RawHeader raw, long baseAddress):
            base(baseAddress)
        {
            _SectorSizeShift = raw.wBlockSize;
            _HashTableOffset = raw.dwHashTablePos;
            _BlockTableOffset = raw.dwBlockTablePos;
            _HashTableEntriesCount = raw.dwHashTableSize;
            _BlockTableEntriesCount = raw.dwBlockTableSize;
        }
    }
}
