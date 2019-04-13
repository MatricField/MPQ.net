//MIT License

//Copyright(c) 2019 Mingxi "Lucien" Du

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

using System.Runtime.InteropServices;

namespace MPQNet.Definition
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class ArchiveHeader1 : ArchiveInfo
    {
        private readonly ushort _SectorSizeShift;
        private readonly uint _HashTableOffset;
        private readonly uint _BlockTableOffset;
        private readonly uint _HashTableEntriesCount;
        private readonly uint _BlockTableEntriesCount;

        /// <summary>
        /// Power of two exponent specifying the number of 512-byte 
        /// disk sectors in each logical sector in the archive.
        /// The size of each logical sector in the archive is
        /// 512 * 2^SectorSizeShift.
        /// </summary>
        public virtual ushort SectorSizeShift => _SectorSizeShift;
        public virtual long SectorSize => 512 << SectorSizeShift;

        /// <summary>
        /// Offset to the beginning of the hash table,
        /// relative to the beginning of the archive.
        /// </summary>
        public virtual long HashTableOffset => _HashTableOffset;

        /// <summary>
        /// Offset to the beginning of the block table,
        /// relative to the beginning of the archive.
        /// </summary>
        public virtual long BlockTableOffset => _BlockTableOffset;

        /// <summary>
        /// Number of entries in the hash table.
        /// </summary>
        public virtual uint HashTableEntriesCount => _HashTableEntriesCount;
        /// <summary>
        /// Number of entries in the block table.
        /// </summary>
        public virtual uint BlockTableEntriesCount => _BlockTableEntriesCount;
    }
}
