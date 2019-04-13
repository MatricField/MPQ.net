using System;
using System.IO;
using System.Threading.Tasks;

namespace MPQNet.IO
{
    public interface ILowLevelIOHandler
    {
        long BaseOffset { get; set; }

        Task<Stream> GetStreamAsync(long offset, long size);

        Stream GetStream(long offset, long size);

        Stream GetFullArchiveStream();
    }
}
