using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace MPQNet.IO
{
    public class ArchiveFileManager :
        IDisposable
    {
        private readonly MemoryMappedFile ArchiveFile;

        private readonly string MapName;

        public ArchiveFileManager(string path)
        {
            MapName = Guid.NewGuid().ToString();
            ArchiveFile = MemoryMappedFile.CreateFromFile(path, FileMode.Open, MapName, 0, MemoryMappedFileAccess.Read);
        }

        public void Dispose()
        {
            ArchiveFile.Dispose();
        }
    }
}
