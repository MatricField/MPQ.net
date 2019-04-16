using System.IO;
using MPQNet.IO.FileIO;

namespace MPQNet.IO
{
    public interface IFileIOHandler
    {
        ILowLevelIOHandler LowLevelIO { get; }
        Stream OpenFile(MPQFileInfo info);
    }
}