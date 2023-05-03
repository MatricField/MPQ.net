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
using System.Runtime.InteropServices;

namespace MPQNet.Definition
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    internal unsafe struct RawHeader
    {
        public const int MD5_DIGEST_SIZE = 0x10;

        // Power of two exponent specifying the number of 512-byte disk sectors in each logical sector
        // in the archive. The size of each logical sector in the archive is 512 * 2^wBlockSize.
        public ushort wBlockSize;

        // Offset to the beginning of the hash table, relative to the beginning of the archive.
        public uint dwHashTablePos;

        // Offset to the beginning of the block table, relative to the beginning of the archive.
        public uint dwBlockTablePos;

        // Number of entries in the hash table. Must be a power of two, and must be less than 2^16 for
        // the original MoPaQ format, or less than 2^20 for the Burning Crusade format.
        public uint dwHashTableSize;

        // Number of entries in the block table
        public uint dwBlockTableSize;

        //-- MPQ HEADER v 2 -------------------------------------------

        // Offset to the beginning of array of 16-bit high parts of file offsets.
        public ulong HiBlockTablePos64;

        // High 16 bits of the hash table offset for large archives.
        public ushort wHashTablePosHi;

        // High 16 bits of the block table offset for large archives.
        public ushort wBlockTablePosHi;

        //-- MPQ HEADER v 3 -------------------------------------------

        // 64-bit version of the archive size
        public ulong ArchiveSize64;

        // 64-bit position of the BET table
        public ulong BetTablePos64;

        // 64-bit position of the HET table
        public ulong HetTablePos64;

        //-- MPQ HEADER v 4 -------------------------------------------

        // Compressed size of the hash table
        public ulong HashTableSize64;

        // Compressed size of the block table
        public ulong BlockTableSize64;

        // Compressed size of the hi-block table
        public ulong HiBlockTableSize64;

        // Compressed size of the HET block
        public ulong HetTableSize64;

        // Compressed size of the BET block
        public ulong BetTableSize64;

        // Size of raw data chunk to calculate MD5.
        // MD5 of each data chunk follows the raw file data.
        public uint dwRawChunkSize;

        // Array of MD5's
        fixed byte _MD5_BlockTable[MD5_DIGEST_SIZE];      // MD5 of the block table before decryption
        public ReadOnlySpan<byte> MD5_BlockTable
        {
            get
            {
                fixed (byte* ptr = _MD5_BlockTable)
                {
                    return new ReadOnlySpan<byte>(ptr, MD5_DIGEST_SIZE);
                }
            }
            init
            {
                fixed (byte* ptr = _MD5_BlockTable)
                {
                    value.CopyTo(new Span<byte>(ptr, MD5_DIGEST_SIZE));
                }
            }
        }

        fixed byte _MD5_HashTable[MD5_DIGEST_SIZE];       // MD5 of the hash table before decryption

        public ReadOnlySpan<byte> MD5_HashTable
        {
            get
            {
                fixed (byte* ptr = _MD5_HashTable)
                {
                    return new ReadOnlySpan<byte>(ptr, MD5_DIGEST_SIZE);
                }
            }
            init
            {
                fixed (byte* ptr = _MD5_HashTable)
                {
                    value.CopyTo(new Span<byte>(ptr, MD5_DIGEST_SIZE));
                }
            }
        }

        fixed byte _MD5_HiBlockTable[MD5_DIGEST_SIZE];    // MD5 of the hi-block table

        public ReadOnlySpan<byte> MD5_HiBlockTable
        {
            get
            {
                fixed (byte* ptr = _MD5_HiBlockTable)
                {
                    return new ReadOnlySpan<byte>(ptr, MD5_DIGEST_SIZE);
                }
            }
            init
            {
                fixed (byte* ptr = _MD5_HiBlockTable)
                {
                    value.CopyTo(new Span<byte>(ptr, MD5_DIGEST_SIZE));
                }
            }
        }

        fixed byte _MD5_BetTable[MD5_DIGEST_SIZE];        // MD5 of the BET table before decryption

        public ReadOnlySpan<byte> MD5_BetTable
        {
            get
            {
                fixed (byte* ptr = _MD5_BetTable)
                {
                    return new ReadOnlySpan<byte>(ptr, MD5_DIGEST_SIZE);
                }
            }
            init
            {
                fixed (byte* ptr = _MD5_BetTable)
                {
                    value.CopyTo(new Span<byte>(ptr, MD5_DIGEST_SIZE));
                }
            }
        }

        fixed byte _MD5_HetTable[MD5_DIGEST_SIZE];        // MD5 of the HET table before decryption

        public ReadOnlySpan<byte> MD5_HetTable
        {
            get
            {
                fixed (byte* ptr = _MD5_HetTable)
                {
                    return new ReadOnlySpan<byte>(ptr, MD5_DIGEST_SIZE);
                }
            }
            init
            {
                fixed (byte* ptr = _MD5_HetTable)
                {
                    value.CopyTo(new Span<byte>(ptr, MD5_DIGEST_SIZE));
                }
            }
        }

        fixed byte _MD5_MpqHeader[MD5_DIGEST_SIZE];       // MD5 of the MPQ header from signature to (including) MD5_HetTable

        public ReadOnlySpan<byte> MD5_MpqHeader
        {
            get
            {
                fixed (byte* ptr = _MD5_MpqHeader)
                {
                    return new ReadOnlySpan<byte>(ptr, MD5_DIGEST_SIZE);
                }
            }
            init
            {
                fixed (byte* ptr = _MD5_MpqHeader)
                {
                    value.CopyTo(new Span<byte>(ptr, MD5_DIGEST_SIZE));
                }
            }
        }

    }
}
