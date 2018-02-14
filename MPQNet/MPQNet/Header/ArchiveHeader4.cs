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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace MPQNet.Header
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class ArchiveHeader4 : ArchiveHeader3, IEquatable<ArchiveHeader4>
    {
        private const int MD5_DIGEST_SIZE = 0x10;

        /// <summary>
        /// Compressed size of the hash table
        /// </summary>
        public ulong HashTableSize64 { get; }

        /// <summary>
        /// Compressed size of the block table
        /// </summary>
        public ulong BlockTableSize64 { get; }

        /// <summary>
        /// Compressed size of the hi-block table
        /// </summary>
        public ulong HiBlockTableSize64 { get; }

        /// <summary>
        /// Compressed size of the HET block
        /// </summary>
        public ulong HetTableSize64 { get; }

        /// <summary>
        /// Compressed size of the BET block
        /// </summary>
        public ulong BetTableSize64 { get; }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MD5_DIGEST_SIZE)]
        private byte[] _MD5_BlockTable;
        /// <summary>
        /// MD5 of the block table before decryption
        /// </summary>
        public IReadOnlyList<byte> MD5_BlockTable => _MD5_BlockTable;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MD5_DIGEST_SIZE)]
        private byte[] _MD5_HashTable;
        /// <summary>
        /// MD5 of the hash table before decryption
        /// </summary>
        public IReadOnlyList<byte> MD5_HashTable => _MD5_HashTable;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MD5_DIGEST_SIZE)]
        private byte[] _MD5_HiBlockTable;
        /// <summary>
        /// MD5 of the hi-block table
        /// </summary>
        public IReadOnlyList<byte> MD5_HiBlockTable => _MD5_HiBlockTable;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MD5_DIGEST_SIZE)]
        private byte[] _MD5_BetTable;
        /// <summary>
        /// MD5 of the BET table before decryption
        /// </summary>
        public IReadOnlyList<byte> MD5_BetTable => _MD5_BetTable;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MD5_DIGEST_SIZE)]
        private byte[] _MD5_HetTable;
        /// <summary>
        /// MD5 of the HET table before decryption
        /// </summary>
        public IReadOnlyList<byte> MD5_HetTable => _MD5_HetTable;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MD5_DIGEST_SIZE)]
        private byte[] _MD5_MpqHeader;
        /// <summary>
        /// MD5 of the MPQ header from signature to (including) MD5_HetTable
        /// </summary>
        public IReadOnlyList<byte> MD5_MpqHeader => _MD5_MpqHeader;

        #region Structural Equality
        public override bool Equals(object obj)
        {
            return Equals(obj as ArchiveHeader4);
        }

        public bool Equals(ArchiveHeader4 other)
        {
            return other != null &&
                   base.Equals(other) &&
                   HashTableSize64 == other.HashTableSize64 &&
                   BlockTableSize64 == other.BlockTableSize64 &&
                   HiBlockTableSize64 == other.HiBlockTableSize64 &&
                   HetTableSize64 == other.HetTableSize64 &&
                   BetTableSize64 == other.BetTableSize64 &&
                   Enumerable.SequenceEqual(MD5_BlockTable, other.MD5_BlockTable) &&
                   Enumerable.SequenceEqual(MD5_HashTable, other.MD5_HashTable) &&
                   Enumerable.SequenceEqual(MD5_HiBlockTable, other.MD5_HiBlockTable) &&
                   Enumerable.SequenceEqual(MD5_BetTable, other.MD5_BetTable) &&
                   Enumerable.SequenceEqual(MD5_HetTable, other.MD5_HetTable) &&
                   Enumerable.SequenceEqual(MD5_MpqHeader, other.MD5_MpqHeader);
        }

        public override int GetHashCode()
        {
            var hashCode = -223769345;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + HashTableSize64.GetHashCode();
            hashCode = hashCode * -1521134295 + BlockTableSize64.GetHashCode();
            hashCode = hashCode * -1521134295 + HiBlockTableSize64.GetHashCode();
            hashCode = hashCode * -1521134295 + HetTableSize64.GetHashCode();
            hashCode = hashCode * -1521134295 + BetTableSize64.GetHashCode();
            hashCode = hashCode * -1521134295 +((IStructuralEquatable)MD5_BlockTable).GetHashCode(EqualityComparer<byte>.Default);
            hashCode = hashCode * -1521134295 +((IStructuralEquatable)MD5_HashTable).GetHashCode(EqualityComparer<byte>.Default);
            hashCode = hashCode * -1521134295 +((IStructuralEquatable)MD5_HiBlockTable).GetHashCode(EqualityComparer<byte>.Default);
            hashCode = hashCode * -1521134295 +((IStructuralEquatable)MD5_BetTable).GetHashCode(EqualityComparer<byte>.Default);
            hashCode = hashCode * -1521134295 +((IStructuralEquatable)MD5_HetTable).GetHashCode(EqualityComparer<byte>.Default);
            hashCode = hashCode * -1521134295 +((IStructuralEquatable)MD5_MpqHeader).GetHashCode(EqualityComparer<byte>.Default);
            return hashCode;
        }

        public static bool operator ==(ArchiveHeader4 header1, ArchiveHeader4 header2)
        {
            return EqualityComparer<ArchiveHeader4>.Default.Equals(header1, header2);
        }

        public static bool operator !=(ArchiveHeader4 header1, ArchiveHeader4 header2)
        {
            return !(header1 == header2);
        } 
        #endregion
    }
}
