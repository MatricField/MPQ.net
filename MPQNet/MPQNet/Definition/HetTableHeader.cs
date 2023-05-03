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

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MPQNet.Definition
{
    /// <summary>
    /// Structure for HET table header
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal class HetTableHeader : ExtTableHeader
    {
        private readonly uint _TableSize;
        private readonly uint _EntryCount;
        private readonly uint _TotalCount;
        private readonly uint _NameHashBitSize;
        private readonly uint _IndexSizeTotal;
        private readonly uint _IndexSizeExtra;
        private readonly uint _IndexSize;
        private readonly uint _IndexTableSize;

        /// <summary>
        /// Size of the entire HET table, including HET_TABLE_HEADER (in bytes)
        /// </summary>
        public uint TableSize => _TableSize;

        /// <summary>
        /// Number of occupied entries in the HET table
        /// </summary>
        public uint EntryCount => _EntryCount;

        /// <summary>
        /// Total number of entries in the HET table
        /// </summary>
        public uint TotalCount => _TotalCount;

        /// <summary>
        /// Size of the name hash entry (in bits)
        /// </summary>
        public uint NameHashBitSize => _NameHashBitSize;

        /// <summary>
        /// Total size of file index (in bits)
        /// </summary>
        public uint IndexSizeTotal => _IndexSizeTotal;

        /// <summary>
        /// Extra bits in the file index
        /// </summary>
        public uint IndexSizeExtra => _IndexSizeExtra;

        /// <summary>
        /// Effective size of the file index (in bits)
        /// </summary>
        public uint IndexSize => _IndexSize;

        /// <summary>
        /// Size of the block index subtable (in bytes)
        /// </summary>
        public uint IndexTableSize => _IndexTableSize;
    }
}
