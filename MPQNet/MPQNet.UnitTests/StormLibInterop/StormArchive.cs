﻿using MPQNet.Definition;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace MPQNet.UnitTests.StormLibInterop
{
    public sealed class StormArchive :
        IDisposable
    {
        private readonly IntPtr StormArchiveHandle;
        private readonly StormArchiveInterop Data;

        /// <summary>
        ///  Position of user data (relative to the begin of the file)
        /// </summary>
        public ulong UserDataPos => Data.UserDataPos;
        /// <summary>
        ///  MPQ header offset (relative to the begin of the file)
        /// </summary>
        public ulong MpqPos => Data.MpqPos;
        /// <summary>
        ///  Size of the file at the moment of file open
        /// </summary>
        public ulong FileSize => Data.FileSize;

        private IReadOnlyList<HashEntry> _HashTable;

        public IReadOnlyList<HashEntry> HashTable
        {
            get
            {
                if(null == _HashTable)
                {
                    var hashTable = new HashEntry[Header.HashTableEntriesCount];
                    var pHashEntry = Data.pHashTable;
                    for(int i = 0; i < hashTable.Length; ++i)
                    {
                        hashTable[i] = Marshal.PtrToStructure<HashEntry>(pHashEntry);
                        pHashEntry += Marshal.SizeOf<HashEntry>();
                    }
                    _HashTable = hashTable;
                }
                return _HashTable;
            }
        }

        /// <summary>
        ///  MPQ user data. Valid only when ID_MPQ_USERDATA has been found
        /// </summary>
        public UserDataHeader UserData => Data.UserData;

        public ArchiveHeader Header => Data.HeaderData;

        public uint dwHETBlockSize => Data.dwHETBlockSize;

        public uint dwBETBlockSize => Data.dwBETBlockSize;

        /// <summary>
        ///  Maximum number of files in the MPQ. Also total size of the file table.
        /// </summary>
        public uint dwMaxFileCount => Data.dwMaxFileCount;

        /// <summary>
        ///  Current size of the file table, e.g. index of the entry past the last occupied one
        /// </summary>
        public uint dwFileTableSize => Data.dwFileTableSize;

        /// <summary>
        ///  Number of entries reserved for internal MPQ files (listfile, attributes)
        /// </summary>
        public uint dwReservedFiles => Data.dwReservedFiles;

        /// <summary>
        ///  Default size of one file sector
        /// </summary>
        public uint dwSectorSize => Data.dwSectorSize;

        /// <summary>
        ///  Flags for (listfile)
        /// </summary>
        public uint dwFileFlags1 => Data.dwFileFlags1;

        /// <summary>
        ///  Flags for (attributes)
        /// </summary>
        public uint dwFileFlags2 => Data.dwFileFlags2;

        /// <summary>
        ///  Flags for (signature)
        /// </summary>
        public uint dwFileFlags3 => Data.dwFileFlags3;

        /// <summary>
        ///  Flags for the (attributes) file, see MPQ_ATTRIBUTE_XXX
        /// </summary>
        public uint dwAttrFlags => Data.dwAttrFlags;

        /// <summary>
        ///  See MPQ_FLAG_XXXXX
        /// </summary>
        public uint dwFlags => Data.dwFlags;

        /// <summary>
        ///  See MPQ_SUBTYPE_XXX
        /// </summary>
        public uint dwSubType => Data.dwSubType;

        /// <summary>
        ///  Amount of bytes that have been processed during a particular compact call
        /// </summary>
        public ulong CompactBytesProcessed => Data.CompactBytesProcessed;

        /// <summary>
        ///  Total amount of bytes to be compacted
        /// </summary>
        public ulong CompactTotalBytes => Data.CompactTotalBytes;

        public StormArchive(string path)
        {
            SFileOpenArchive(path, out StormArchiveHandle);
            Data = Marshal.PtrToStructure<StormArchiveInterop>(StormArchiveHandle);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        private void DoDispose()
        {
            if (!disposedValue)
            {
                SFileCloseArchive(StormArchiveHandle);
                disposedValue = true;
            }
        }

        ~StormArchive()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            DoDispose();
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            DoDispose();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Interop

        [DllImport(@"StormLib.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern bool SFileOpenArchive(
            [MarshalAs(UnmanagedType.LPStr)]string filename,
            uint prioerity,
            uint openFlags,
            out IntPtr phMPQ);

        [DllImport(@"StormLib.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern bool SFileCloseArchive(IntPtr hMPQ);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool SFileOpenArchive(string filename, out IntPtr phMPQ)
        {
            return SFileOpenArchive(filename, 0, 0, out phMPQ);
        }
        #endregion
    }
}
