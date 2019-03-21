using MPQNet.Definition;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MPQNet.UnitTests.StormLib
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
            OpenArchive(path, out StormArchiveHandle);
            Data = Marshal.PtrToStructure<StormArchiveInterop>(StormArchiveHandle);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        private void DoDispose()
        {
            if (!disposedValue)
            {
                CloseArchive(StormArchiveHandle);
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
        private static class Win32
        {
            [DllImport(@"Lib\StormLibWin32.dll", CallingConvention = CallingConvention.Winapi)]
            public static extern bool SFileOpenArchive(
                [MarshalAs(UnmanagedType.LPStr)]string filename,
                uint prioerity,
                uint openFlags,
                out IntPtr phMPQ);

            [DllImport(@"Lib\StormLibWin32.dll", CallingConvention = CallingConvention.Winapi)]
            public static extern bool SFileCloseArchive(IntPtr hMPQ);
        }

        private static class Win64
        {
            [DllImport(@"Lib\StormLibWin64.dll", CallingConvention = CallingConvention.Winapi)]
            public static extern bool SFileOpenArchive(
                [MarshalAs(UnmanagedType.LPStr)]string filename,
                uint prioerity,
                uint openFlags,
                out IntPtr phMPQ);

            [DllImport(@"Lib\StormLibWin64.dll", CallingConvention = CallingConvention.Winapi)]
            public static extern bool SFileCloseArchive(IntPtr hMPQ);
        }

        private static bool CloseArchive(IntPtr hMPQ)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                switch (RuntimeInformation.ProcessArchitecture)
                {
                    case Architecture.X86:
                        return Win32.SFileCloseArchive(hMPQ);
                    case Architecture.X64:
                        return Win64.SFileCloseArchive(hMPQ);
                }
            }
            throw new NotSupportedException();
        }

        private static bool OpenArchive(string filename, out IntPtr phMPQ)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                switch (RuntimeInformation.ProcessArchitecture)
                {
                    case Architecture.X86:
                        return Win32.SFileOpenArchive(filename, 0, 0, out phMPQ);
                    case Architecture.X64:
                        return Win64.SFileOpenArchive(filename, 0, 0, out phMPQ);
                }
            }
            throw new NotSupportedException();
        }
        #endregion
    }
}
