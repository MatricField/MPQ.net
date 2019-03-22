using MPQNet.Definition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MPQNet.UnitTests.StormLibInterop
{
    [StructLayout(LayoutKind.Sequential)]
    internal sealed class StormArchiveInterop
    {
        private const int MPQ_HEADER_SIZE_V4 = 0xD0;
        /// <summary>
        ///  Open stream for the MPQ
        /// </summary>
        private IntPtr pStream { get; }
        /// <summary>
        ///  Position of user data (relative to the begin of the file)
        /// </summary>
        public ulong UserDataPos { get; }
        /// <summary>
        ///  MPQ header offset (relative to the begin of the file)
        /// </summary>
        public ulong MpqPos { get; }
        /// <summary>
        ///  Size of the file at the moment of file open
        /// </summary>
        public ulong FileSize { get; }

        /// <summary>
        /// Pointer to patch archive, if any
        /// </summary>
        private IntPtr _haPatch;

        /// <summary>
        /// Pointer to base ("previous version") archive, if any
        /// </summary>
        private IntPtr haBase;

        /// <summary>
        ///  Patch prefix to precede names of patch files
        /// </summary>
        private IntPtr pPatchPrefix;

        /// <summary>
        ///  MPQ user data (NULL if not present in the file)
        /// </summary>
        private IntPtr pUserData;

        /// <summary>
        ///  MPQ file header
        /// </summary>
        private IntPtr pHeader;

        /// <summary>
        ///  Hash table
        /// </summary>
        public IntPtr pHashTable { get; }

        /// <summary>
        ///  HET table
        /// </summary>
        private IntPtr pHetTable;

        /// <summary>
        ///  File table
        /// </summary>
        private IntPtr pFileTable;

        /// <summary>
        ///  Hashing function that will convert the file name into hash
        /// </summary>
        private IntPtr pfnHashString;

        /// <summary>
        ///  MPQ user data. Valid only when ID_MPQ_USERDATA has been found
        /// </summary>
        public UserDataHeader UserData;

        public ArchiveHeader4 HeaderData { get; }  // Storage for MPQ header

        public uint dwHETBlockSize { get; }
        public uint dwBETBlockSize { get; }
        /// <summary>
        ///  Maximum number of files in the MPQ. Also total size of the file table.
        /// </summary>
        public uint dwMaxFileCount { get; }
        /// <summary>
        ///  Current size of the file table, e.g. index of the entry past the last occupied one
        /// </summary>
        public uint dwFileTableSize { get; }
        /// <summary>
        ///  Number of entries reserved for internal MPQ files (listfile, attributes)
        /// </summary>
        public uint dwReservedFiles { get; }
        /// <summary>
        ///  Default size of one file sector
        /// </summary>
        public uint dwSectorSize { get; }
        /// <summary>
        ///  Flags for (listfile)
        /// </summary>
        public uint dwFileFlags1 { get; }
        /// <summary>
        ///  Flags for (attributes)
        /// </summary>
        public uint dwFileFlags2 { get; }
        /// <summary>
        ///  Flags for (signature)
        /// </summary>
        public uint dwFileFlags3 { get; }
        /// <summary>
        ///  Flags for the (attributes) file, see MPQ_ATTRIBUTE_XXX
        /// </summary>
        public uint dwAttrFlags { get; }
        /// <summary>
        ///  See MPQ_FLAG_XXXXX
        /// </summary>
        public uint dwFlags { get; }
        /// <summary>
        ///  See MPQ_SUBTYPE_XXX
        /// </summary>
        public uint dwSubType { get; }
        /// <summary>
        ///  Callback function for adding files
        /// </summary>
        private IntPtr pfnAddFileCB;
        /// <summary>
        ///  User data thats passed to the callback
        /// </summary>
        private IntPtr pvAddFileUserData { get; }
        /// <summary>
        ///  Callback function for compacting the archive
        /// </summary>
        private IntPtr pfnCompactCB;
        /// <summary>
        ///  Amount of bytes that have been processed during a particular compact call
        /// </summary>
        public ulong CompactBytesProcessed { get; }
        /// <summary>
        ///  Total amount of bytes to be compacted
        /// </summary>
        public ulong CompactTotalBytes { get; }
        /// <summary>
        ///  User data thats passed to the callback
        /// </summary>
        private IntPtr pvCompactUserData { get; }
    }
}
