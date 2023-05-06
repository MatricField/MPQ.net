using MPQNet.Definition;
using System;
using System.Collections.Generic;
using System.IO;
using SubstreamSharp;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using MPQNet.Cryptography;
using MPQNet.IO;
using MPQNet.Compression;
using Ionic.BZip2;
using System.IO.Compression;

namespace MPQNet.ArchiveDetails
{
    internal class Archive1 :
        IArchive
    {
        protected readonly long BaseAddress;
        protected readonly string ArchivePath;
        private byte[] _HashTable;
        protected virtual Span<HashEntry> HashTable => MemoryMarshal.Cast<byte, HashEntry>(_HashTable);
        protected byte[] _BlockTable;
        protected virtual Span<BlockEntry> BlockTable => MemoryMarshal.Cast<byte,  BlockEntry>(_BlockTable);

        protected List<string> _FileList;

        protected static byte[] LoadMPQTable<TEntry>(Stream stream, int count, uint compressedSize, uint decryptKey)
            where TEntry : struct
        {
            var entrySize = Marshal.SizeOf<TEntry>();
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
            var buffer = new byte[tableRawSize];
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            if(bytesRead != buffer.Length)
            {
                throw new InvalidDataException();
            }
            return buffer;
        }

        public Archive1(string archivePath, Header header, UserDataHeader userDataHeader)
        {
            ArchivePath = archivePath;
            BaseAddress = header.BaseAddress;
            var header4 = header as Header4;
            using (var archiveStream = FileIO.OpenShared(archivePath))
            {
                //Load hash table
                var hashTableCompressedSize = (uint)header4.HashTableSize64;
                var substream = archiveStream.Substream(
                        header4.BaseAddress + header4.HashTableOffset,
                        hashTableCompressedSize);
                _HashTable = LoadMPQTable<HashEntry>(
                    substream,
                    (int)header4.HashTableEntriesCount,
                    hashTableCompressedSize,
                    SpecialFiles.HashTableKey);
                archiveStream.Seek(0, SeekOrigin.Begin);
                //Load block table
                var blockTableCompressedSize = (uint)header4.BlockTableSize64;
                substream = archiveStream.Substream(
                    header4.BaseAddress + header4.BlockTableOffset,
                    blockTableCompressedSize);
                _BlockTable = LoadMPQTable<BlockEntry>(
                    substream,
                    (int)header4.BlockTableEntriesCount,
                    blockTableCompressedSize,
                    SpecialFiles.BlockTableKey);
            }
            _FileList = new List<string>();
            if(TryOpenFile(SpecialFiles.ListFile, out var listFile))
            {
                using(var streamReader = new StreamReader(listFile))
                {
                    string line;
                    while((line = streamReader.ReadLine()) != null)
                    {
                        _FileList.Add(line);
                    }
                }
            }
        }

        #region IArchive
        public bool CanEnumerateFile => _FileList != null;

        public IEnumerable<string> EnumerateFile => _FileList;

        public bool HasUserData => throw new NotImplementedException();

        public Stream GetUserDataStream()
        {
            throw new NotImplementedException();
        }

        public bool TryOpenFile(string path, out Stream result)
        {
            var dwIndex = HashString.HashDefault(path, HashType.TableIndex);
            var dwName1 = HashString.HashDefault(path, HashType.NameA);
            var dwName2 = HashString.HashDefault(path, HashType.NameB);
            for(int i = (int)dwIndex & (HashTable.Length - 1); ; i++)
            {
                ref readonly var hashEntry = ref HashTable[i];
                if(hashEntry.Name1 == dwName1 && hashEntry.Name2 == dwName2)
                {
                    ref readonly var blockEntry = ref BlockTable[hashEntry.BlockIndex];
                    var stream = FileIO.OpenShared(ArchivePath);
                    var fileBlock = stream.Substream(BaseAddress+blockEntry.FilePos, blockEntry.FileSize);
                    if(blockEntry.Flags.HasFlag(MPQFileFlags.SINGLE_UNIT))
                    {
                        if(blockEntry.Flags.HasFlag(MPQFileFlags.COMPRESS))
                        {
                            var compressionMask = (CompressionFlags)fileBlock.ReadByte();
                            var fileData = stream.Substream(BaseAddress + blockEntry.FilePos + 1, blockEntry.FileSize - 1, ownUnderlyingStream: true);
                            switch (compressionMask)
                            {
                                case CompressionFlags.BZIP2:
                                    result = new BZip2InputStream(fileData);
                                    return true;
                                case CompressionFlags.DEFLATED:
                                    result = new DeflateStream(fileData, CompressionMode.Decompress);
                                    return true;
                                case CompressionFlags.HUFFMANN:
                                case CompressionFlags.IMPLODED:
                                case CompressionFlags.LZMA:
                                case CompressionFlags.SPARSE:
                                case CompressionFlags.ADPCM_MONO:
                                case CompressionFlags.ADPCM_STEREO:
                                case CompressionFlags.NEXT_SAME:
                                    // TODO: Add more compression methods
                                    throw new NotImplementedException();
                                default:
                                    throw new NotSupportedException();
                            }
                        }
                    }
                }
                else if(hashEntry.BlockIndex == HashEntry.BLOCK_INDEX_EMPTY_END)
                {
                    result = null;
                    return false;
                }
            }
        }

        public Stream OpenFile(string path)
        {
            if(TryOpenFile(path, out var file))
            {
                return file;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        #endregion
    }
}
