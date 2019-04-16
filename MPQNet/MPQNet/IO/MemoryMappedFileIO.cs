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
        private const FileMode FILE_MODE = FileMode.Open;

        private const FileShare FILE_SHARE = FileShare.Read;

        private string _Path;

        private MemoryMappedFile _ArchiveFile;

        public string MemoryMapName { get; }

        public MemoryMappedFileIO(string path, long archiveFileOffset = 0)
        {
            _Path = path;
            MemoryMapName = Guid.NewGuid().ToString();

            _ArchiveFile = MemoryMappedFile.CreateFromFile(path, FILE_MODE, MemoryMapName, 0, MemoryMappedFileAccess.Read);
            BaseOffset = archiveFileOffset;
        }

        public override Stream GetFullArchiveStream()
        {
            return _ArchiveFile.CreateViewStream(0, 0, MemoryMappedFileAccess.Read);
        }

        public override Stream GetStream(long offset, long size) =>
            _ArchiveFile.CreateViewStream(BaseOffset + offset, size, MemoryMappedFileAccess.Read);

        public override Task<Stream> GetStreamAsync(long offset, long size) =>
            Task.FromResult(GetStream(offset, size));

        protected override void DoDispose(bool disposing)
        {
            if(disposing)
            {
                _ArchiveFile.Dispose();
            }
            base.DoDispose(disposing);
        }
    }
}
