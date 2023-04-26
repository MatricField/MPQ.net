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

namespace MPQNet.Definition
{
    internal record class Header4: Header3
    {
        private ulong _HashTableSize64;
        private ulong _BlockTableSize64;
        private ulong _HiBlockTableSize64;
        private ulong _HetTableSize64;
        private ulong _BetTableSize64;

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

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = MD5_DIGEST_SIZE)]
        //private byte[] _MD5_BlockTable;
        ///// <summary>
        ///// MD5 of the block table before decryption
        ///// </summary>
        //public IReadOnlyList<byte> MD5_BlockTable => _MD5_BlockTable;

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = MD5_DIGEST_SIZE)]
        //private byte[] _MD5_HashTable;
        ///// <summary>
        ///// MD5 of the hash table before decryption
        ///// </summary>
        //public IReadOnlyList<byte> MD5_HashTable => _MD5_HashTable;

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = MD5_DIGEST_SIZE)]
        //private byte[] _MD5_HiBlockTable;
        ///// <summary>
        ///// MD5 of the hi-block table
        ///// </summary>
        //public IReadOnlyList<byte> MD5_HiBlockTable => _MD5_HiBlockTable;

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = MD5_DIGEST_SIZE)]
        //private byte[] _MD5_BetTable;
        ///// <summary>
        ///// MD5 of the BET table before decryption
        ///// </summary>
        //public IReadOnlyList<byte> MD5_BetTable => _MD5_BetTable;

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = MD5_DIGEST_SIZE)]
        //private byte[] _MD5_HetTable;
        ///// <summary>
        ///// MD5 of the HET table before decryption
        ///// </summary>
        //public IReadOnlyList<byte> MD5_HetTable => _MD5_HetTable;

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = MD5_DIGEST_SIZE)]
        //private byte[] _MD5_MpqHeader;
        ///// <summary>
        ///// MD5 of the MPQ header from signature to (including) MD5_HetTable
        ///// </summary>
        //public IReadOnlyList<byte> MD5_MpqHeader => _MD5_MpqHeader;
    }
}
