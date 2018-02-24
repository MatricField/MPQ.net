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

using MPQNet.Definition;
using MPQNet.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MPQNet
{
    public class Archive
    {
        protected const int FORMAT_VERSION_OFFSET = 0xC;

        protected FileInfo ArchiveFile { get; private set; }

        public long ArchiveOffset { get; protected set; }

        public UserDataHeader UserDataHeader { get; protected set; }

        public ArchiveHeader Header { get; protected set; }

        public IReadOnlyList<HashEntry> HashTable { get; protected set; }
        public IReadOnlyList<BlockEntry> BlockTable { get; protected set; }

        public Archive()
        {

        }

        public Archive(string path)
        {
            try
            {
                var task = Init(path);
                task.Wait();
            }
            catch (AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
        }

        public async Task Init(string path)
        {
            ArchiveFile = new FileInfo(path);
            using (var stream = ArchiveFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await LoadHeaderAsync(stream);
                await LoadHashTableAsync(stream);
                await LoadBlockTableAsync(stream);
            }
        }

        protected virtual async Task LoadHeaderAsync(Stream stream)
        {
            var inputReader = new BinaryReader(stream);
            try
            {
                for (; null == Header ; )
                {
                    var headerSignature = (Signatures)inputReader.ReadUInt32();
                    stream.Seek(-sizeof(Signatures), SeekOrigin.Current);
                    switch (headerSignature)
                    {
                        case Signatures.MPQ_UserData:
                            var userDataHeaderOffset = stream.Position;
                            UserDataHeader = await stream.MarshalObjectFromStreamAsync<UserDataHeader>();
                            stream.Seek(userDataHeaderOffset + UserDataHeader.HeaderOffset, SeekOrigin.Begin);
                            break;
                        case Signatures.MPQ:
                            ArchiveOffset = stream.Position;
                            stream.Seek(FORMAT_VERSION_OFFSET, SeekOrigin.Current);
                            var formatVersion = (FormatVersions)inputReader.ReadUInt16();
                            stream.Seek(-FORMAT_VERSION_OFFSET - sizeof(FormatVersions), SeekOrigin.Current);
                            await ReadMPQHeaderAsync(formatVersion, inputReader);
                            break;
                        default:
                            stream.Seek(0x200, SeekOrigin.Current);
                            break;
                    }
                }
            }
            catch (EndOfStreamException)
            {
                throw new InvalidDataException();
            }
        }

        protected virtual async Task ReadMPQHeaderAsync(FormatVersions version, BinaryReader inputReader)
        {
            var stream = inputReader.BaseStream;
            switch (version)
            {
                case FormatVersions.V1:
                    Header = await stream.MarshalObjectFromStreamAsync<ArchiveHeader>();
                    break;
                default:
                    throw new NotSupportedException("NotSupported format version");
            }
        }

        protected virtual async Task LoadHashTableAsync(Stream stream)
        {
            stream.Seek(ArchiveOffset + Header.HashTableOffset, SeekOrigin.Begin);
            var data = new byte[Header.HashTableEntriesCount * Marshal.SizeOf<HashEntry>()];
            stream.Read(data, 0, data.Length);
            MPQCryptor.DecryptDataInplace(data, TableInfo.HashTableKey);
            var memstream = new MemoryStream(data);
            var hashTable = new HashEntry[Header.HashTableEntriesCount];
            for (int i = 0; i < Header.HashTableEntriesCount; ++i)
            {
                hashTable[i] = await memstream.MarshalObjectFromStreamAsync<HashEntry>();
            }
            HashTable = hashTable;
        }

        protected virtual async Task LoadBlockTableAsync(Stream stream)
        {
            stream.Seek(ArchiveOffset + Header.BlockTableOffset, SeekOrigin.Begin);
            var data = new byte[Header.BlockTableEntriesCount * Marshal.SizeOf<BlockEntry>()];
            stream.Read(data, 0, data.Length);
            MPQCryptor.DecryptDataInplace(data, TableInfo.BlockTableKey);
            var memstream = new MemoryStream(data);
            var blockTable = new BlockEntry[Header.BlockTableEntriesCount];
            for (int i = 0; i < Header.BlockTableEntriesCount; ++i)
            {
                blockTable[i] = await memstream.MarshalObjectFromStreamAsync<BlockEntry>();
            }
            BlockTable = blockTable;
        }


    }
}
