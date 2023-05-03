using MPQNet.Definition;
using System;
using System.Collections.Generic;
using System.IO;
using SubstreamSharp;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using MPQNet.Cryptography;
using MPQNet.IO;
using MPQNet.Compression;
using Ionic.BZip2;

namespace MPQNet.ArchiveDetails
{
    internal class Archive1 :
        IArchive
    {
        protected readonly long BaseAddress;
        protected readonly string ArchivePath;
        protected List<HashEntry> _HashTable;
        protected List<BlockEntry> _BlockTable;

        protected static List<TEntry> LoadMPQTable<TRaw, TEntry>(Stream stream, int count, uint compressedSize, uint decryptKey, Func<TRaw, TEntry> Convert)
            where TRaw : struct
        {
            var entrySize = Marshal.SizeOf<TRaw>();
            if(decryptKey != 0)
            {
                stream = new CryptoStream(stream,
                    new DataBlockCypher(decryptKey),
                    CryptoStreamMode.Read);
            }
            var tableRawSize = entrySize * count;
            if(compressedSize < tableRawSize)
            {
                throw new NotImplementedException();
            }
            var buffer = new byte[entrySize].AsSpan();
            var result = new List<TEntry>(capacity: count);
            for(; ; )
            {
                var bytesRead = stream.Read(buffer);
                if (bytesRead == buffer.Length)
                {
                    var rawEntry = MemoryMarshal.AsRef<TRaw>(buffer);
                    result.Add(Convert(rawEntry));
                }
                else if (bytesRead != 0)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        public Archive1(string archivePath, Header header, UserDataHeader userDataHeader)
        {
            ArchivePath = archivePath;
            BaseAddress = header.BaseAddress;
            var header4 = header as Header4;
            _HashTable = new List<HashEntry>(capacity: (int)header4.HashTableEntriesCount);
            using (var archiveStream = Archive.OpenFile(archivePath))
            {
                var hashTableCompressedSize = (uint)header4.HashTableSize64;
                var substream = archiveStream.Substream(
                        header4.BaseAddress + header4.HashTableOffset,
                        hashTableCompressedSize);
                _HashTable = LoadMPQTable<RawHashEntry, HashEntry>(
                    substream,
                    (int)header4.HashTableEntriesCount,
                    hashTableCompressedSize,
                    SpecialFiles.HashTableKey,
                    raw => new HashEntry(raw));
            }
            using (var archiveStream = Archive.OpenFile(archivePath))
            {
                var blockTableCompressedSize = (uint)header4.BlockTableSize64;
                var substream = archiveStream.Substream(
                    header4.BaseAddress + header4.BlockTableOffset,
                    blockTableCompressedSize);
                _BlockTable = LoadMPQTable<RawBlockEntry, BlockEntry>(
                    substream,
                    (int)header4.BlockTableEntriesCount,
                    blockTableCompressedSize,
                    SpecialFiles.BlockTableKey,
                    raw => new BlockEntry(raw));
            }
        }

        #region IArchive
        public bool CanEnumerateFile => throw new NotImplementedException();

        public IEnumerable<string> EnumerateFile => throw new NotImplementedException();

        public bool HasUserData => throw new NotImplementedException();

        public Stream GetUserDataStream()
        {
            throw new NotImplementedException();
        }

        public Stream OpenFile(string path)
        {
            var dwIndex = HashString.HashDefault(path, HashType.TableIndex);
            var dwName1 = HashString.HashDefault(path, HashType.NameA);
            var dwName2 = HashString.HashDefault(path, HashType.NameB);
            for(int i = (int)dwIndex & (_HashTable.Count - 1); ; i++)
            {
                var hashEntry = _HashTable[i];
                if(hashEntry.Name1 == dwName1 && hashEntry.Name2 == dwName2)
                {
                    var blockEntry = _BlockTable[hashEntry.BlockIndex];
                    var stream = Archive.OpenFile(ArchivePath);
                    var fileBlock = stream.Substream(BaseAddress+blockEntry.FilePos, blockEntry.FileSize);
                    if(blockEntry.Flags.HasFlag(MPQFileFlags.SINGLE_UNIT))
                    {
                        if(blockEntry.Flags.HasFlag(MPQFileFlags.COMPRESS))
                        {
                            var compressionMask = (CompressionFlags)fileBlock.ReadByte();
                            switch(compressionMask)
                            {
                                case CompressionFlags.BZIP2:
                                    return new BZip2InputStream(stream.Substream(BaseAddress + blockEntry.FilePos + 1, blockEntry.FileSize - 1));
                            }
                        }
                    }
                }
                else if(hashEntry.BlockIndex == HashEntry.BLOCK_INDEX_EMPTY_END)
                {
                    throw new FileNotFoundException();
                }
            }
            throw new NotImplementedException();
        }

        public bool TryOpenFile(string path, out Stream stream)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
