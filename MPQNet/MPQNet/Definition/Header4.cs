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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MPQNet.Definition
{
    internal record class Header4 : Header3
    {
        protected ulong _HashTableSize64;
        protected ulong _BlockTableSize64;
        protected ulong _HiBlockTableSize64;
        protected ulong _HetTableSize64;
        protected ulong _BetTableSize64;
        protected byte[] _MD5_BlockTable;
        protected byte[] _MD5_HashTable;
        protected byte[] _MD5_HiBlockTable;
        protected byte[] _MD5_BetTable;
        protected byte[] _MD5_HetTable;
        protected byte[] _MD5_MpqHeader;

        /// <summary>
        /// Compressed size of the hash table
        /// </summary>
        public required virtual ulong HashTableSize64
        {
            get => _HashTableSize64;
            init => _HashTableSize64 = value;
        }

        /// <summary>
        /// Compressed size of the block table
        /// </summary>
        public required virtual ulong BlockTableSize64
        {
            get => _BlockTableSize64;
            init => _BlockTableSize64 = value;
        }

        /// <summary>
        /// Compressed size of the hi-block table
        /// </summary>
        public required virtual ulong HiBlockTableSize64
        {
            get => _HiBlockTableSize64;
            init => _HiBlockTableSize64 = value;
        }

        /// <summary>
        /// Compressed size of the HET block
        /// </summary>
        public required virtual ulong HetTableSize64
        {
            get => _HetTableSize64;
            init => _HashTableSize64 = value;
        }

        /// <summary>
        /// Compressed size of the BET block
        /// </summary>
        public required virtual ulong BetTableSize64
        {
            get => _BetTableSize64;
            init => _BetTableSize64 = value;
        }


        /// <summary>
        /// MD5 of the block table before decryption
        /// </summary>
        public required ReadOnlySpan<byte> MD5_BlockTable 
        { 
            get => _MD5_BlockTable;
            init => _MD5_BlockTable = value.ToArray();
        }


        /// <summary>
        /// MD5 of the hash table before decryption
        /// </summary>
        public required ReadOnlySpan<byte> MD5_HashTable
        {
            get => _MD5_HashTable;
            init => _MD5_HashTable = value.ToArray();
        }


        /// <summary>
        /// MD5 of the hi-block table
        /// </summary>
        public required ReadOnlySpan<byte> MD5_HiBlockTable
        {
            get => _MD5_HiBlockTable;
            init => _MD5_HiBlockTable = value.ToArray();
        }


        /// <summary>
        /// MD5 of the BET table before decryption
        /// </summary>
        public required ReadOnlySpan<byte> MD5_BetTable
        {
            get => _MD5_BetTable;
            init => _MD5_BetTable = value.ToArray();
        }


        /// <summary>
        /// MD5 of the HET table before decryption
        /// </summary>
        public required ReadOnlySpan<byte> MD5_HetTable
        {
            get => _MD5_HetTable;
            init => _MD5_HetTable = value.ToArray();
        }


        /// <summary>
        /// MD5 of the MPQ header from signature to (including) MD5_HetTable
        /// </summary>
        public required ReadOnlySpan<byte> MD5_MpqHeader
        {
            get => _MD5_MpqHeader;
            init => _MD5_MpqHeader = value.ToArray();
        }

        public Header4()
            : base()
        {

        }

        [SetsRequiredMembers]
        public Header4(in RawHeader raw, long baseAddress)
            : base(raw, baseAddress)
        {
            _HashTableSize64 = raw.HashTableSize64;
            _BlockTableSize64 = raw.BlockTableSize64;
            _HiBlockTableSize64 = raw.HiBlockTableSize64;
            _HetTableSize64 = raw.HetTableSize64;
            _BetTableSize64 = raw.BetTableSize64;
            _MD5_BlockTable = raw.MD5_BlockTable.ToArray();
            _MD5_HashTable = raw.MD5_HashTable.ToArray();
            _MD5_HiBlockTable = raw.MD5_HiBlockTable.ToArray();
            _MD5_BetTable = raw.MD5_BetTable.ToArray();
            _MD5_HetTable = raw.MD5_HetTable.ToArray();
            _MD5_MpqHeader = raw.MD5_MpqHeader.ToArray();
        }
    }
}
