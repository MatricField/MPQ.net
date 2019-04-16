using MPQNet.Definition;
using MPQNet.IO;
using MPQNet.IO.FileIO;
using System;
using System.Collections.Generic;
using System.IO;

namespace MPQNet
{

    public class MPQArchive :
        IArchive
    {
        private IFileIOHandler IOHandler;
        private IMPQHashTable hashTable;
        private List<string> fileList;
        private UserDataInfo userDataInfo;

        public bool CanEnumerateFile => fileList != null;

        public bool HasUserData => userDataInfo.UserDataSize > 0;

        public MPQArchive(IFileIOHandler IOHandler, IMPQHashTable hashTable, in UserDataInfo userData = default)
        {
            if (IOHandler == null || hashTable == null)
            {
                throw new ArgumentNullException();
            }
            this.userDataInfo = userData;
            this.IOHandler = IOHandler;
            this.hashTable = hashTable;
            if (TryOpenFile(SpecialFiles.ListFile, out var listFileStream))
            {
                using (listFileStream)
                {
                    fileList = new List<string>();
                    var reader = new StreamReader(listFileStream);
                    var line = reader.ReadLine();
                    while (null != line)
                    {
                        fileList.Add(line);
                        line = reader.ReadLine();
                    }
                }
            }
        }

        public Stream OpenFile(string path)
        {
            if (TryOpenFile(path, out var stream))
            {
                return stream;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public bool TryOpenFile(string path, out Stream stream)
        {
            if (hashTable.TryGetValue(path, out var block))
            {
                var info = new MPQFileInfo()
                {
                    Name = Path.GetFileName(path),
                    Block = block
                };
                stream = IOHandler.OpenFile(info);
                return true;
            }
            else
            {
                stream = default;
                return false;
            }
        }

        public IEnumerable<string> EnumerateFile =>
            fileList ?? throw new InvalidOperationException();

        public Stream GetUserDataStream()
        {
            if (!HasUserData)
            {
                throw new InvalidOperationException();
            }
            return IOHandler.LowLevelIO.GetStream(userDataInfo.UserDataOffset, userDataInfo.UserDataSize);
        }
    }
}
