using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading.Tasks;

namespace MPQNet.IO
{
    public class MemoryMappedFileIO:
        LowLevelIOHandlerBase
    {
        private MemoryMappedFile _ArchiveFile;

        public MemoryMappedFileIO(MemoryMappedFile getArchiveFileFunc, long archiveFileOffset)
        {
            _ArchiveFile = getArchiveFileFunc;
            BaseOffset = archiveFileOffset;
        }

        public override Stream GetStream(long offset, long size) =>
            _ArchiveFile.CreateViewStream(BaseOffset + offset, size);

        public override Task<Stream> GetStreamAsync(long offset, long size) =>
            Task.FromResult(GetStream(offset, size));
    }
}
