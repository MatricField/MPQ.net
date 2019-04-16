using MPQNet.Definition;
using MPQNet.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace MPQNet
{
    public abstract class MPQArchiveBase:
        IArchive
    {
        private ILowLevelIOHandler IOHandler;
        private IMPQHashTable hashTable;
        private List<string> fileList;
        public UserDataHeader UserDataHeader { get; }

        public bool CanEnumerateFile => fileList != null;

        public MPQArchiveBase(ILowLevelIOHandler IOHandler, IMPQHashTable hashTable, UserDataHeader userData = null)
        {
            if(IOHandler == null || hashTable == null)
            {
                throw new ArgumentNullException();
            }
            this.UserDataHeader = userData;
            this.IOHandler = IOHandler;
            this.hashTable = hashTable;
            if(TryOpenFile(SpecialFiles.ListFile, out var listFileStream))
            {
                using (listFileStream)
                {
                    fileList = new List<string>();
                    var reader = new StreamReader(listFileStream);
                    var line = reader.ReadLine();
                    while(null != line)
                    {
                        fileList.Add(line);
                        line = reader.ReadLine();
                    }
                }
            }
        }

        public Stream OpenFile(string path)
        {
            if(TryOpenFile(path, out var stream))
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
            if(hashTable.TryGetValue(path, out var info))
            {
                stream = OpenFileStream(info);
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

        protected abstract Stream OpenFileStream(IMPQFileInfo info);
    }
}
