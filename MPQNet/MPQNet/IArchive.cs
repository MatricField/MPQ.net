using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MPQNet
{
    public interface IArchive
    {
        bool CanEnumerateFile { get; }
        IEnumerable<string> EnumerateFile { get; }
        bool HasUserData { get; }

        Stream GetUserDataStream();
        Stream OpenFile(string path);
        bool TryOpenFile(string path, out Stream stream);
    }
}
