using System;
using System.IO;
using System.Threading.Tasks;

namespace MPQNet.IO.LowLevelIO
{
    public class GeneralStreamIO :
        LowLevelIOHandlerBase
    {
        private Func<Stream> GetArchiveFile;

        public GeneralStreamIO(Func<Stream> getArchiveFileFunc, long archiveStartOffset = 0)
        {
            GetArchiveFile = getArchiveFileFunc;
            BaseOffset = archiveStartOffset;
        }

        public override Stream GetFullArchiveStream()
        {
            return GetArchiveFile();
        }

        public override Stream GetStream(long offset, long size)
        {
            var archiveFile = GetArchiveFile();
            var buffer = new MemoryStream();
            archiveFile.Seek(BaseOffset + offset, SeekOrigin.Begin);
            while (size > 0)
            {
                var toCopy = size <= int.MaxValue ? (int)size : int.MaxValue;
                archiveFile.CopyTo(archiveFile, toCopy);
                size -= toCopy;
            }
            return buffer;
        }

        public override async Task<Stream> GetStreamAsync(long offset, long size)
        {
            var archiveFile = GetArchiveFile();
            var buffer = new MemoryStream();
            archiveFile.Seek(BaseOffset + offset, SeekOrigin.Begin);
            while (size > 0)
            {
                var toCopy = size <= int.MaxValue ? (int)size : int.MaxValue;
                await archiveFile.CopyToAsync(archiveFile, toCopy);
                size -= toCopy;
            }
            return buffer;
        }
    }
}
