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
    /// Structure for BET table header
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class BetTableHeader : ExtTableHeader
    {
        /// <summary>
        /// Size of the entire BET table, including the header (in bytes)
        /// </summary>
        public uint TableSize { get; }

        /// <summary>
        /// Number of entries in the BET table. Must match HET_TABLE_HEADER::dwEntryCount
        /// </summary>
        public uint EntryCount { get; }


        public uint Unknown08 { get; }

        /// <summary>
        /// Size of one table entry (in bits)
        /// </summary>
        public uint TableEntrySize { get; }

        /// <summary>
        /// Bit index of the file position (within the entry record)
        /// </summary>
        public uint BitIndex_FilePos { get; }

        /// <summary>
        /// Bit index of the file size (within the entry record)
        /// </summary>
        public uint BitIndex_FileSize { get; }

        /// <summary>
        /// Bit index of the compressed size (within the entry record)
        /// </summary>
        public uint BitIndex_CmpSize { get; }

        /// <summary>
        /// Bit index of the flag index (within the entry record)
        /// </summary>
        public uint BitIndex_FlagIndex { get; }

        /// <summary>
        /// Bit index of the ??? (within the entry record)
        /// </summary>
        public uint BitIndex_Unknown { get; }

        /// <summary>
        /// Bit size of file position (in the entry record)
        /// </summary>
        public uint BitCount_FilePos { get; }

        /// <summary>
        /// Bit size of file size (in the entry record)
        /// </summary>
        public uint BitCount_FileSize { get; }

        /// <summary>
        /// Bit size of compressed file size (in the entry record)
        /// </summary>
        public uint BitCount_CmpSize { get; }

        /// <summary>
        /// Bit size of flags index (in the entry record)
        /// </summary>
        public uint BitCount_FlagIndex { get; }

        /// <summary>
        /// Bit size of ??? (in the entry record)
        /// </summary>
        public uint BitCount_Unknown { get; }

        /// <summary>
        /// Total bit size of the NameHash2
        /// </summary>
        public uint BitTotal_NameHash2 { get; }

        /// <summary>
        /// Extra bits in the NameHash2
        /// </summary>
        public uint BitExtra_NameHash2 { get; }

        /// <summary>
        /// Effective size of NameHash2 (in bits)
        /// </summary>
        public uint BitCount_NameHash2 { get; }

        /// <summary>
        /// Size of NameHash2 table, in bytes
        /// </summary>
        public uint NameHashArraySize { get; }

        /// <summary>
        /// Number of flags in the following array
        /// </summary>
        public uint FlagCount { get; }
    }
}
