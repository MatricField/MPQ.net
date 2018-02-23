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

        public ArchiveHeader2 Header2 { get; protected set; }
        public ArchiveHeader3 Header3 { get; protected set; }
        public ArchiveHeader4 Header4 { get; protected set; }

        public IReadOnlyList<HashEntry> HashTable { get; protected set; }
        public IReadOnlyList<BlockEntry> BlockTable { get; protected set; }

        public static async Task<Archive> Create(string path)
        {
            var result = new Archive();
            result.ArchiveFile = new FileInfo(path);
            using (var stream = result.ArchiveFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await result.ReadHeaderAsync(stream);
                throw new NotImplementedException();
            }
            return result;
        }

        protected Archive()
        {

        }

        public Archive(string path)
        {
            ArchiveFile = new FileInfo(path);
            using (var stream = ArchiveFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ReadHeader(stream);
                LoadHashTable(stream);
            }
        }

        protected virtual void ReadHeader(Stream stream)
        {
            try
            {
                var task = ReadHeaderAsync(stream);
                task.Wait();
            }
            catch(AggregateException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
        }

        protected virtual async Task ReadHeaderAsync(Stream stream)
        {
            var inputReader = new BinaryReader(stream);
            try
            {
                for (; ; )
                {
                    var headerSignature = (Signatures)inputReader.ReadUInt32();
                    switch (headerSignature)
                    {
                        case Signatures.MPQ_UserData:
                            stream.Seek(-sizeof(Signatures), SeekOrigin.Current);
                            var userDataHeaderOffset = stream.Position;
                            UserDataHeader = await stream.MarshalObjectFromStreamAsync<UserDataHeader>();
                            stream.Seek(userDataHeaderOffset, SeekOrigin.Begin);
                            stream.Seek(UserDataHeader.HeaderOffset, SeekOrigin.Current);
                            break;
                        case Signatures.MPQ:
                        case Signatures.MPK:
                            stream.Seek(-sizeof(Signatures), SeekOrigin.Current);
                            ArchiveOffset = stream.Position;
                            stream.Seek(FORMAT_VERSION_OFFSET, SeekOrigin.Current);
                            var formatVersion = (FormatVersions)inputReader.ReadUInt16();
                            stream.Seek(-FORMAT_VERSION_OFFSET - sizeof(FormatVersions), SeekOrigin.Current);
                            switch (formatVersion)
                            {
                                case FormatVersions.V1:
                                    Header = await stream.MarshalObjectFromStreamAsync<ArchiveHeader>();
                                    break;
                                case FormatVersions.V2:
                                    Header = Header2 = await stream.MarshalObjectFromStreamAsync<ArchiveHeader2>();
                                    break;
                                case FormatVersions.V3:
                                    Header = Header2 = Header3 = await stream.MarshalObjectFromStreamAsync<ArchiveHeader3>();
                                    break;
                                case FormatVersions.V4:
                                    Header = Header2 = Header3 = Header4 = await stream.MarshalObjectFromStreamAsync<ArchiveHeader4>();
                                    break;
                                default:
                                    throw new NotSupportedException("NotSupported format version");
                            }
                            return;
                        default:
                            stream.Seek(0x200 - sizeof(Signatures), SeekOrigin.Current);
                            break;
                    }
                }
            }
            catch (EndOfStreamException)
            {
                throw new InvalidDataException();
            }
        }

        protected virtual void LoadHashTable(Stream stream)
        {
            stream.Seek(ArchiveOffset + Header.HashTableOffset, SeekOrigin.Begin);
            var data = new byte[Header.HashTableEntriesCount * Marshal.SizeOf<HashEntry>()];
            stream.Read(data, 0, data.Length);
            MPQCryptor.DecryptDataInplace(data, TableInfo.HashKey);
            var memstream = new MemoryStream(data);
            var hashTable = new List<HashEntry>();
            for (int i = 0; i < Header.HashTableEntriesCount; ++i)
            {
                hashTable.Add(memstream.MarshalObjectFromStream<HashEntry>());
            }
            HashTable = hashTable;
        }

        protected virtual void LoadBlockTable(Stream stream)
        {
            throw new NotImplementedException();
        }


    }
}
