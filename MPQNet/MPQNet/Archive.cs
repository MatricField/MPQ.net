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
using System.IO.MemoryMappedFiles;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MPQNet
{
    public class Archive :
        IDisposable
    {
        protected const int FORMAT_VERSION_OFFSET = 0xC;

        protected MemoryMappedFile ArchiveFile { get; private set; }

        protected string MemoryMapName { get; private set; }

        public long ArchiveOffset { get; protected set; }

        public UserDataHeader UserDataHeader { get; protected set; }

        public ArchiveHeader Header { get; protected set; }

        public IReadOnlyList<HashEntry> HashTable { get; private set; }
        public IReadOnlyList<BlockEntry> BlockTable { get; private set; }

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
            MemoryMapName = Guid.NewGuid().ToString("B");
            ArchiveFile = MemoryMappedFile.CreateFromFile(path, FileMode.Open, MemoryMapName, 0, MemoryMappedFileAccess.Read);
            using (var stream = GetStreamView(0, 0))
            {
                await LoadHeaderAsync(stream);
            }

            var hashTableTask = LoadHashTableAsync();
            var blockTableTask = LoadBlockTableAsync();
            HashTable = await hashTableTask;
            BlockTable = await blockTableTask;
        }

        public virtual Stream GetStreamView(long offset, long size)
        {
            return ArchiveFile.CreateViewStream(offset, size, MemoryMappedFileAccess.Read);
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

        protected virtual async Task<IReadOnlyList<HashEntry>> LoadHashTableAsync()
        {
            var count = (int)Header.HashTableEntriesCount;
            var size = count * Marshal.SizeOf<HashEntry>();
            return await ReadTableAsync<HashEntry>(GetStreamView(ArchiveOffset + Header.HashTableOffset, size), count, TableInfo.HashTableKey);
        }

        protected virtual async Task<IReadOnlyList<BlockEntry>> LoadBlockTableAsync()
        {
            var count = (int)Header.BlockTableEntriesCount;
            var size = count * Marshal.SizeOf<BlockEntry>();
            return await ReadTableAsync<BlockEntry>(GetStreamView(ArchiveOffset + Header.BlockTableOffset, size), count, TableInfo.BlockTableKey);
        }

        private async Task<T[]> ReadTableAsync<T>(Stream stream, int count, uint key)
        {
            var data = new byte[count * Marshal.SizeOf<T>()];
            await stream.ReadAsync(data, 0, data.Length);
            MPQCryptor.DecryptDataInplace(data, key);
            return data.MarshalArrayFromBuffer<T>(count);
        }

        protected virtual BlockEntry? FindBlock(string path)
        {
            var index = MPQCryptor.HashString(path, HashType.TableOffset);
            var name1 = MPQCryptor.HashString(path, HashType.NameA);
            var name2 = MPQCryptor.HashString(path, HashType.NameB);
            for(var i = index & (HashTable.Count - 1); ; ++i)
            {
                //TODO: make this[long]
                var currentBlock = HashTable[(int)i];
                if(currentBlock.Name1 == name1 &&
                    currentBlock.Name2 == name2)
                {
                    if(currentBlock.BlockIndex == HashEntry.HASH_ENTRY_NO_LONGER_VALID)
                    {
                        return null;
                    }
                    return BlockTable[currentBlock.BlockIndex];
                }
                if(currentBlock.BlockIndex == HashEntry.HASH_ENTRY_IS_EMPTY)
                {
                    return null;
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ArchiveFile?.Dispose();
                }
                ArchiveFile = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
