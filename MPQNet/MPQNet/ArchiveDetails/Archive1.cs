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

namespace MPQNet.ArchiveDetails
{
    internal class Archive1 :
        IArchive
    {
        protected List<HashEntry> _HashTable;
        protected List<BlockEntry> _BlockTable;

        public Archive1(string archivePath, Header header, UserDataHeader userDataHeader)
        {
            var header4 = header as Header4;
            _HashTable = new List<HashEntry>(capacity: (int)header4.HashTableEntriesCount);
            using (var archiveStream = Archive.OpenFile(archivePath))
            {
                var hashTableRawSize = header4.HashTableEntriesCount * Marshal.SizeOf<RawHashEntry>();
                var hashTableCompressedSize = (long)header4.HashTableSize64;
                Stream hashTableStream = archiveStream.Substream(
                    header4.BaseAddress + header4.HashTableOffset,
                    hashTableCompressedSize);
                hashTableStream = new CryptoStream(
                    hashTableStream,
                    new DataBlockCypher(SpecialFiles.HashTableKey),
                    CryptoStreamMode.Read);
                if (hashTableCompressedSize < hashTableRawSize)
                {
                    throw new NotImplementedException();
                }
                var buffer = new byte[Marshal.SizeOf<RawHashEntry>()].AsSpan();
                for (; ; )
                {
                    var bytesRead = hashTableStream.Read(buffer);
                    if (bytesRead == buffer.Length)
                    {
                        var rawHashEntry = MemoryMarshal.AsRef<RawHashEntry>(buffer);
                        _HashTable.Add(new HashEntry(rawHashEntry));
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
            throw new NotImplementedException();
        }

        public bool TryOpenFile(string path, out Stream stream)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
